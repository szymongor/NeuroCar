using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PolygonCollisionMT
{
    class NeuroCarControl
    {
        private NeuralNetwork _nn;
        Car _car;
        bool _stopThread;
        bool readyToDrive;
        double speed;
        double angle;
        private Thread NeuroCarControlThread;

        public NeuroCarControl()
        {
            readyToDrive = false;
        }

        public void setCar(Car car)
        {
            _car = car;
        }

        public void setNeuralNetwork(NeuralNetwork nn)
        {
            _nn = nn;
            _car.setSensorsNumber(nn.InputSize);
        }

        public void setControl(int inputSize, int innerSize, int innerNumber, double activationCoefficient)
        {
            stopThread();
            _car.setSensorsNumber(inputSize);
            _nn = new NeuralNetwork(inputSize+2, innerSize, 2, innerNumber, activationCoefficient);
            readyToDrive = true;
            startThread();
        }

        public void setControl(NeuralNetwork nn)
        {
            stopThread();
            _car.setSensorsNumber(nn.InputSize-2);
            _nn = nn;
            readyToDrive = true;
            startThread();
        }

        public void threadAction(object data)
        {
            while (!_stopThread)
            {

                if (readyToDrive )
                {
                    MyVector sensorVec = _car.getSensorVector();
                    MyVector controlVec = _nn.answer(sensorVec);
                    double angle = MyMath.mapOnRange(controlVec[0] * 0.01, 0.01);
                    double speed = MyMath.mapOnRange(controlVec[1], 0.21);
                    _car.control(angle,speed);
                }

                //if (_car.getHp() < 0)
                //{
                //    stopThread();
                //}

                Thread.Sleep(30);
            }
        }

        public void startThread()
        {
            _stopThread = false;
            NeuroCarControlThread = new Thread(this.threadAction);
            NeuroCarControlThread.Start();
            NeuroCarControlThread.Name = "NeuroCarControlThread";
        }

        public void stopThread()
        {
            if (NeuroCarControlThread != null)
            {
                //NeuroCarControlThread.Abort();
                _stopThread = true;
                readyToDrive = false;
            }
        }

    }
}
