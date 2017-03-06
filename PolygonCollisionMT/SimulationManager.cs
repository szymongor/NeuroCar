using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PolygonCollisionMT
{
    public class SimulationManager
    {

        SimulationManagerView _SMView;
        List<Simulation> _simulationList;
        GeneticAlgorithm<Simulation> GA;
        int _timeLimit;
        int _boundaryX;
        int _roadLength;
        int _numberofSimulations;
        int raceCount = 0;
        static int raceLimit = 5;

        long _threadTime;

        int _progress;

        Thread simulationManagerThread;

        public SimulationManager()
        {
            _progress = 0;
            _simulationList = new List<Simulation>();
            GA = new GeneticAlgorithm<Simulation>();
            startThread();
        }

        public Simulation newSimulation( int boundaryX, int roadLength, int inputSize
            , int innerSize, int innerNumber, double activationCoefficient)
        {

            string name = "Simulation " + (_simulationList.Count + 1);
            Simulation sim = new Simulation(boundaryX, roadLength, inputSize, innerSize,
                innerNumber, activationCoefficient, name);

            return sim;
        }

        public Simulation newSimulation(NeuralNetwork nn, int boundaryX,
            int roadLength)
        {
            string name = "Simulation " + (_simulationList.Count + 1);
            Simulation sim = new Simulation(boundaryX, roadLength, nn, name);

            return sim;
        }

        public void startNewSimulations(int numberOfSimulations, int timeLimit,
            int boundaryX, int roadLength, int inputSize
            , int innerSize, int innerNumber, double activationCoefficient)
        {          
            _timeLimit = timeLimit;
            _boundaryX = boundaryX;
            _roadLength = roadLength;
            _numberofSimulations = numberOfSimulations;
            for (int i = 0; i < numberOfSimulations; i++)
            {
                _simulationList.Add(newSimulation(boundaryX, roadLength, inputSize
            , innerSize, innerNumber, activationCoefficient));
            }
            _SMView.nextGeneration(0);
            _SMView.updateView();
        }

        public void startNewSimulations(List<NeuralNetwork> nns, int boundaryX,
            int roadLength)
        {            
            foreach (NeuralNetwork nn in nns)
            {
                _simulationList.Add(newSimulation(nn, boundaryX, roadLength));
            }
            _SMView.nextGeneration((1.0 / (double)raceLimit));
            _SMView.updateView();
        }

        public bool chceckEndOfSimulations()
        {
            bool returnValue = true;
            try
            {
                foreach (Simulation sim in _simulationList)
                {
                    if (!sim.isFinished() && (sim.getTime() >= _timeLimit || sim.isDestroyed()))
                    {
                        sim.stopThreads();
                        _progress += (int)(100 / _simulationList.Count);
                    }
                    if (!sim.isFinished())
                    {
                        returnValue = false;
                    }
                }
            }
            catch (System.InvalidOperationException)
            {
                return returnValue;
            }
            return returnValue;
        }

        public void threadAction(object data)
        {
            while (true)
            {
                TimeSpan span = new TimeSpan(DateTime.Now.Ticks - _threadTime);
                if(span.TotalMilliseconds > 300)
                {
                    if (_simulationList.Count != 0 && chceckEndOfSimulations())
                    {
                        if (raceCount == raceLimit)
                        {
                            raceCount = 0;
                            newBreed();
                        }
                        else
                        {
                            raceCount++;
                            nextRace();
                        }
                    }
                    _threadTime = DateTime.Now.Ticks;
                }

            }        
        }

        public void startThread()
        {
            _threadTime = DateTime.Now.Ticks;
            simulationManagerThread = new Thread(this.threadAction);
            simulationManagerThread.Start();
            simulationManagerThread.Name = "SimulationManagerThread";

        }

        public void stopSimulations()
        {
            foreach (Simulation sim in _simulationList)
            {
                sim.stopThreads();

                int i = 0;
            }
            //_simulationList = new List<Simulation>();
        }

        public void stopThreads()
        {
            foreach (Simulation sim in _simulationList)
            {
                sim.stopThreads();
            }
            simulationManagerThread.Abort();
        }

        public void newBreed()
        {
            List<NeuralNetwork> neuralNetworkList = new List<NeuralNetwork>();
            List<int> pointsList = new List<int>();

            GA.setGeneration(_simulationList);

            List<Simulation> newBreed = GA.getNewGeneration();


            foreach (Simulation sim in newBreed)
            {
                neuralNetworkList.Add(sim.getNeuralNetwork());
            }

           
            _SMView.setBestScore(GA.getBestScore()/raceLimit);
            stopSimulations();
            _simulationList.Clear();
            GC.Collect();
            _progress = 0;          

            startNewSimulations(neuralNetworkList, _boundaryX, _roadLength);
        }

        public void nextRace()
        {
            List<NeuralNetwork> neuralNetworkList = new List<NeuralNetwork>();
            List<int> pointsList = new List<int>();

            GA.setGeneration(_simulationList);

            List<Simulation> newBreed = GA.getGeneration();


            foreach (Simulation sim in newBreed)
            {
                neuralNetworkList.Add(sim.getNeuralNetwork());
            }

            //_SMView.setBestScore(GA.getBestScore());

            List<int> scoreList = new List<int>();
            foreach (Simulation sim in _simulationList)
            {
                scoreList.Add(sim.Score);
            }
            stopSimulations();
            _simulationList.Clear();
            GC.Collect();
            _progress = 0;

            startNewSimulations(neuralNetworkList, _boundaryX, _roadLength);
            for (int i = 0; i < _simulationList.Count; i++)
            {
                _simulationList[i].updateScore(scoreList[i]);
            }
        }

        public Simulation getSimulation(int i)
        {
            if (i >= _simulationList.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return _simulationList[i];
        }

        public Simulation getSimulation()
        {
            return _simulationList[_simulationList.Count-1];
        }

        public List<Simulation> getSimulationList()
        {
            return _simulationList;
        }

        public void setView(SimulationManagerView SMView)
        {
            _SMView = SMView;
        }

        public int getProgress()
        {
            return _progress;
        }
    }
}
