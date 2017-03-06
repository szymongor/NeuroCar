using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolygonCollisionMT
{
    public class RangeFinder
    {
        private List<MyVector> _directionVectors;
        private PolygonManager _polygonManager;
        private double _radiusLength;
        public int _radiusDivisons;
        private int _radiusNumber;
        public double divisonLength
        {
            get
            {
                return _radiusLength / _radiusDivisons;
            }
        }

        public RangeFinder(int radiusNumber, double radiusLength, int radiusDivisons)
        {
            _radiusLength = radiusLength;
            _radiusDivisons = radiusDivisons;
            _radiusNumber = radiusNumber;
            _directionVectors = new List<MyVector>();
            double angleIncrement = (2 * Math.PI) / radiusNumber;
            for (int i = 0; i < radiusNumber; i++)
            {
                MyVector directionVec = new MyVector(Math.Sin(angleIncrement * i), Math.Cos(angleIncrement * i));
                _directionVectors.Add(directionVec);
            }
        }

        public void setPolygonManager(PolygonManager pm)
        {
            _polygonManager = pm;
        }

        public int rangeInDirection(MyVector directionVector, MyVector positionPoint, List<Polygon> polygons)
        {

            for (int i = 0; i < _radiusDivisons; i++)
            {
                MyVector pointToCheck = positionPoint + directionVector * divisonLength * i;
                foreach (Polygon p in polygons)
                {
                    if ( p.isPointInside(pointToCheck) ||
                        _polygonManager.checkPointBoundaryCollision(pointToCheck) )
                    {
                        return i;
                    }
                }
            }
            return _radiusDivisons;
        }
        
        public MyVector rangeInAllDirections(MyVector positionPoint, List<Polygon> polygons, MyVector frontVec)
        {
            MyVector rangeVector = new MyVector();
            List<MyVector> directionVectors = getDirectionVectors(frontVec);
            foreach (MyVector direction in directionVectors)
            {
                rangeVector.add(rangeInDirection(direction, positionPoint, polygons));
            }
            return rangeVector;
             
            
        }

        public List<MyVector> sensorsToDraw()
        {
            List<MyVector> sensors = new List<MyVector>();
            
            for (int i = 0; i < _directionVectors.Count; i++)
            {
                MyVector sensorPosition = _directionVectors[i] * _radiusLength;
                sensors.Add(sensorPosition);
            }

            return sensors;
        }

        public void setDirection(MyVector frontVector)
        {
            double startAngle = MyMath.polarRepresentation(frontVector)[1];
            double angleIncrement = (2 * Math.PI) / _radiusNumber;
            _directionVectors.Clear();
            for (int i = 0; i < _radiusNumber; i++)
            {
                MyVector directionVec = new MyVector(Math.Sin(startAngle + angleIncrement * i),
                    Math.Cos(startAngle + angleIncrement * i));
                _directionVectors.Add(directionVec);
            }

        }

        public List<MyVector> getDirectionVectors(MyVector frontVector)
        {
            double startAngle = MyMath.polarRepresentation(frontVector)[1];
            double angleIncrement = (2 * Math.PI) / _radiusNumber;
            List<MyVector> directionVectors = new List<MyVector>();
            for (int i = 0; i < _radiusNumber; i++)
            {
                MyVector directionVec = new MyVector(Math.Sin(startAngle + angleIncrement * i),
                    Math.Cos(startAngle + angleIncrement * i));
                directionVectors.Add(directionVec);
            }
            return directionVectors;
        }
    }
}
