using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCollisionMT
{
    public class Simulation : IGenetic<Simulation>
    {
        PolygonManager _polygonMng;
        Car _car;
        Road _road;
        NeuralNetwork _nn;
        NeuroCarControl _neuroControl;

        bool _isFinished;
        long _startTime;
        int _stopTime;

        int _boundaryX;
        int _roadLength;
        string _name;
        int totalScore = 0;

        public int Score
        {
            get
            {
                return totalScore + _car.getDistanceScore();
            }
        }

        public Simulation(int boundaryX, int roadLength)
        {
            _boundaryX = boundaryX;
            _roadLength = roadLength;
            _name = "Auto";
            _car = new Car();
            _polygonMng = new PolygonManager(boundaryX, roadLength, _car);
            _car.setPolygonManager(_polygonMng);

            _road = new Road(roadLength);
            _road.generatePolygons(10);
            _polygonMng.setRoad(_road);

            _nn = new NeuralNetwork(5, 3, 2, 2, 0.5);

            _neuroControl = new NeuroCarControl();

            _neuroControl.setCar(_car);
            _neuroControl.setControl(5, 3, 2, 1);
            _startTime = DateTime.Now.Ticks;
        }

        public Simulation(int boundaryX, int roadLength, int inputSize
            , int innerSize, int innerNumber, double activationCoefficient,
            string name)
        {
            _boundaryX = boundaryX;
            _roadLength = roadLength;
            _name = name;
            _car = new Car();
            _polygonMng = new PolygonManager(boundaryX, roadLength, _car);
            _car.setPolygonManager(_polygonMng);

            _road = new Road(roadLength);
            _road.generatePolygons((int)roadLength/100);
            _polygonMng.setRoad(_road);

            _nn = new NeuralNetwork(inputSize+2, innerSize, 2, innerNumber, activationCoefficient);

            _neuroControl = new NeuroCarControl();

            _neuroControl.setCar(_car);
            _neuroControl.setControl(_nn);
            _startTime = DateTime.Now.Ticks;
        }

        public Simulation(int boundaryX, int roadLength, NeuralNetwork nn,
            string name)
        {
            _boundaryX = boundaryX;
            _roadLength = roadLength;
            _name = name;
            _car = new Car();
            _polygonMng = new PolygonManager(boundaryX, roadLength, _car, "name");
            _car.setPolygonManager(_polygonMng);

            _road = new Road(roadLength);
            _road.generatePolygons((int)roadLength / 100);
            _polygonMng.setRoad(_road);

            _nn = nn;

            _neuroControl = new NeuroCarControl();

            _neuroControl.setCar(_car);
            _neuroControl.setControl(_nn);
            _startTime = DateTime.Now.Ticks;
        }

        public PolygonManager getPolygonManager()
        {
            return _polygonMng;
        }

        public Road getRoad()
        {
            return _road;
        }

        public Car getCar()
        {
            return _car;
        }

        public NeuralNetwork getNeuralNetwork()
        {
            return _nn;
        }

        public void stopThreads()
        {
            if (!_isFinished)
            {
                _polygonMng.stopThread();
                _neuroControl.stopThread();

                _isFinished = true;

                TimeSpan span = new TimeSpan(DateTime.Now.Ticks - _startTime);


                _stopTime = (int)span.TotalSeconds;
            }
        }

        public int getHP()
        {
            return _car.getHp();
        }

        public bool isDestroyed()
        {
            return _car.isDestroyed();
        }

        public int getTime()
        {
            if (isFinished())
                return _stopTime;

            TimeSpan span = new TimeSpan(DateTime.Now.Ticks - _startTime);

            return (int)span.TotalSeconds;
        }

        public bool isFinished()
        {
            return _isFinished;
        }

        public override string ToString()
        {
            return _name;
        }

        public Simulation mutate()
        {
            NeuralNetwork mutatedNN = _nn.getMutatedNeuralNetwork();

            Simulation sim = new Simulation(_boundaryX, _roadLength, mutatedNN, "Temp");
            sim.stopThreads();
            return sim;
        }

        public void updateScore(int score)
        {
            totalScore += score;
        }
    }
}
