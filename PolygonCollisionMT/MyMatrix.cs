using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCollisionMT
{
    public class MyMatrix
    {
        private List<MyVector> _matrix;

        public List<MyVector> GetMatrix
        {
            get
            {
                return _matrix;
            }

        }
        public int RowsNumber
        {
            get
            {
                return _matrix.Count();
            }
        }

        public int ColumnsNumber
        {
            get
            {
                return _matrix[0].Length;
            }
        }

        public MyMatrix()
        {
            _matrix = new List<MyVector>();
        }

        public MyMatrix(MyMatrix matrix)
        {
            _matrix = new List<MyVector>();
            for (int i = 0; i < matrix.RowsNumber; i++)
            {
                MyVector vec = new MyVector(matrix[i]);
                _matrix.Add(vec);
            }
        }

        public MyMatrix(List<MyVector> m)
        {
            _matrix = m;
        }

        public MyMatrix(int n, bool isRandom, double randomRange)
        {
            _matrix = new List<MyVector>();
            if (isRandom)
            {
                for (int i = 0; i < n; i++)
                {
                    _matrix.Add(new MyVector(n, true, randomRange));
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    _matrix.Add(new MyVector(n, false, randomRange));
                }
            }
        }

        public MyMatrix(int rows, int columns, bool isRandom, double randomRange) // wektor długości c, lista długości rows
        {
            _matrix = new List<MyVector>();
            if (isRandom)
            {
                for (int i = 0; i < rows; i++)
                {
                    _matrix.Add(new MyVector(columns, true, randomRange));
                }
            }
            else
            {
                for (int i = 0; i < rows; i++)
                {
                    _matrix.Add(new MyVector(columns, false, randomRange));
                }
            }
        }

        public void add(MyVector value)
        {
            MyVector tempMv = new MyVector(value.GetVector);
            _matrix.Add(tempMv);
        }

        public void clear()
        {
            _matrix.Clear();
        }

        public MyVector this[int index]
        {
            get
            {
                return this._matrix[index];
            }

        }

        public static MyMatrix operator +(MyMatrix m1, MyMatrix m2)
        {
            if (m1.RowsNumber != m2.RowsNumber || m1.ColumnsNumber != m2.ColumnsNumber)
                throw new FormatException();

            MyMatrix result = new MyMatrix();
            for (int i = 0; i < m1.RowsNumber; i++)
            {
                result.add(m1[i] + m2[i]);
            }
            return result;
        }

        public override string ToString()
        {
            string matrix = "";
            for (int i = 0; i < this.RowsNumber; i++)
            {
                matrix += this[i].ToString() + Environment.NewLine;
            }
            return matrix;
        }
    }
}
