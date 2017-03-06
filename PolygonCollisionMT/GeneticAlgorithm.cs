using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCollisionMT
{
    class GeneticAlgorithm<T> where T : class, IGenetic<T>
    {
        List<T> _generation;

        double _selectionRatio;

        public GeneticAlgorithm()
        {
            _selectionRatio = 0.7;
        }

        public void setGeneration(List<T> generation)
        {
            _generation = generation;
        }

        public void sortGenerationByScore()
        {
            _generation.Sort(CompareByScore);
        }

        public int getBestScore()
        {
            return _generation.Max(x => x.Score);
        }

        public List<T> getGeneration()
        {
            List<T> newGenerationList = new List<T>();
            int selectionNumber = (int)(_generation.Count * _selectionRatio);

            for (int i = 0; i < selectionNumber; i++)
            {
                newGenerationList.Add(_generation[i]);
            }
            for (int i = selectionNumber; i < _generation.Count; i++)
            {
                newGenerationList.Add(_generation[i - selectionNumber].mutate());
            }
            return newGenerationList;
        }

        public List<T> getNewGeneration()
        {
            List<T> newGenerationList = new List<T>();
            sortGenerationByScore();
            int selectionNumber = (int)(_generation.Count * _selectionRatio);

            for (int i = 0; i < selectionNumber; i++)
            {
                newGenerationList.Add(_generation[i]);
            }
            for (int i = selectionNumber; i < _generation.Count; i++)
            {
                newGenerationList.Add(_generation[i - selectionNumber].mutate());
            }
            return newGenerationList;
        }

        private static int CompareByScore(T x, T y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (y == null)
                {
                    return -1;
                }
                else
                {
                    if (x.Score < y.Score)
                    {
                        return 1;
                    }
                    else if (x.Score == y.Score)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

    }
}
