using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PolygonCollisionMT
{
    public class NeuralNetwork
    {
        private List<MyMatrix> _layers;
        private MyVector _currentAnswer;
        private double _coefficientActivationFunction;
        private double _neuralMatrixRange = 1;
        private double _mutationRange = 0.1666;
        private double _mutationProbability = 0.1;

        public List<MyMatrix> GetLayers
        {
            get
            {
                return _layers;
            }
        }

        public MyVector CurrentAnswer
        {
            get
            {
                return _currentAnswer;
            }
            set
            {
                _currentAnswer = value;
            }
        }

        public int InputSize
        {
            get
            {
                return _layers[0].RowsNumber-1;
            }
        }

        public NeuralNetwork()
        {
            _layers = new List<MyMatrix>();
            _coefficientActivationFunction = 1;
        }

        public NeuralNetwork(NeuralNetwork nn)
        {
            _layers = new List<MyMatrix>(nn._layers);
            _coefficientActivationFunction = nn._coefficientActivationFunction;
        }

        public NeuralNetwork(int firstLayerSize, int innerLayerSize, int lastLayerSize, int innerLayerNumber, double coefficientActivationFunction)
        {
            _coefficientActivationFunction = coefficientActivationFunction;
            _layers = new List<MyMatrix>();

            MyMatrix firstMatrix = new MyMatrix(firstLayerSize + 1, innerLayerSize, true, _neuralMatrixRange);
            _layers.Add(firstMatrix);
            for (int i = 0; i < innerLayerNumber; i++)
            {
                MyMatrix tempMatrix = new MyMatrix(innerLayerSize + 1, innerLayerSize, true, _neuralMatrixRange);
                _layers.Add(tempMatrix);
            }
            MyMatrix lastMatrix = new MyMatrix(innerLayerSize + 1, lastLayerSize, true, _neuralMatrixRange);
            _layers.Add(lastMatrix);
        }

        public NeuralNetwork getMutatedNeuralNetwork()
        {
            NeuralNetwork mutatedNN = new NeuralNetwork();

            for (int i = 0; i < _layers.Count; i++)
            {
                MyMatrix matrix = new MyMatrix(_layers[i]);
                for(int row=0; row<matrix.RowsNumber; row++)
                {
                    for(int c=0; c<matrix[row].Length; c++)
                    {
                        if(MyMath.randDouble(0,1)<_mutationProbability)
                        {
                            double rand = MyMath.randDouble((-1)*_mutationRange,_mutationRange);
                            matrix[row][c] += rand;
                        }    
                    }
                }
                mutatedNN.add(matrix);
            }
            return mutatedNN;
        }

        public MyVector answer(MyVector inputVector)
        {
            inputVector.add(1);

            if (inputVector.Length != _layers[0].RowsNumber)
            {
                throw new Exception();
            }
            else
            {
                MyVector tempV = inputVector * _layers[0];
                tempV = MyMath.activationFunction(tempV, _coefficientActivationFunction);
                tempV.add(1);
                int i;
                for (i = 1; i < _layers.Count() - 1; i++)
                {
                    tempV = tempV * _layers[i];
                    tempV = MyMath.activationFunction(tempV, _coefficientActivationFunction);
                    tempV.add(1);
                }
                _currentAnswer = tempV * _layers[i];
                return _currentAnswer;
            }
        }

        public void add(MyMatrix value)
        {
            _layers.Add(value);
        }

        public void clear()
        {
            _layers.Clear();
        }

        public string showLayers()
        {
            string layers = "";
            foreach (var l in _layers)
            {
                layers += l.ToString() + Environment.NewLine + Environment.NewLine;
            }
            return layers;
        }
    }
}
