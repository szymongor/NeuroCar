using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolygonCollisionMT
{
    public class Car
    {
        public Triangle CarShape { get; set; }
        private MyVector front;
        private MyVector frontVector
        {
            get
            {
                return MyMath.normalizeVector(CarShape[2] - CarShape.MassCentre);
            }
        }
        RangeFinder rangeFinder;
        PolygonManager polygonManager;
        private int HP;
        bool _destroyed;
        private int distScore;

        double x1 = 178, x2 = 208, x3, y1 = 240, y2 = 240, y3 = 280;
        public Car()
        {
            _destroyed = false;
            x3 = (x1 + x2) / 2;
            PointVector pv = new PointVector(x1, y1, x2, y2, x3, y3);
            MyVector velocity = new MyVector(0, 0);
            CarShape = new Triangle(pv, velocity);
            front = CarShape[2];
            rangeFinder = new RangeFinder(10, 150, 20);
            HP = 100;
            distScore = 0;
        }

        public void setPolygonManager(PolygonManager pm)
        {
            rangeFinder.setPolygonManager(pm);
            polygonManager = pm;
        }

        public void control(double rotationAngle, double speed)
        {
            CarShape.forward(speed);
            CarShape._angularVelocity += rotationAngle;
        }

        public void setSensorsNumber(int sn)
        {
            rangeFinder = new RangeFinder(sn, 150, 20);
            rangeFinder.setPolygonManager(polygonManager);
        }

        public MyVector getDistanceVector()
        {
            MyVector position = CarShape.MassCentre;
            List<Polygon> polygonsList = polygonManager.getPolygonList();
            return rangeFinder.rangeInAllDirections(position, polygonsList, frontVector);
        }

        public MyVector getSensorVector()
        {
            MyVector sensorVector = new MyVector();
            MyVector velocity = new MyVector(frontVector);
            MyVector distanceVector = getDistanceVector();
            sensorVector.join(velocity);
            sensorVector.join(distanceVector);
            return sensorVector;
        }

        public void damageCar()
        {
            HP -= 11;
            if (HP < 0)
            {
                _destroyed = true;
            }
        }

        public int getHp()
        {
            return HP;
        }

        public bool isDestroyed()
        {
            return _destroyed;
        }

        public int getDistanceScore()
        {
            if(distScore < (int)CarShape.MassCentre[1])
                distScore = (int)CarShape.MassCentre[1];

            return distScore;
        }

        public List<List<Point>> sensorsToDraw()
        {
            rangeFinder.setDirection(frontVector); // przenieść gdzieś żeby się cały czas robiło a nie tylko podczas rysowania.

            List<MyVector> sensorsPoints = rangeFinder.sensorsToDraw();
            List<List<Point>> linesToDraw = new List<List<Point>>();

            Point carPosition = new Point((int)CarShape.MassCentre[0], (int)CarShape.MassCentre[1]);

            for(int i = 0 ; i < sensorsPoints.Count ; i ++)
            {
                Point sensorPoint = new Point((int)(carPosition.X + sensorsPoints[i][0]), (int)(carPosition.Y + sensorsPoints[i][1]));
                List<Point> sensorLine = new List<Point>();
                sensorLine.Add(carPosition);
                sensorLine.Add(sensorPoint);
                linesToDraw.Add(sensorLine);
            }

            return linesToDraw;
        }

        public List<Point> measurmentsToDraw()
        {
            
            List<MyVector> sensorsPoints = rangeFinder.sensorsToDraw();
            List<Point> measurmentsToDraw = new List<Point>();
            MyVector measurments = getDistanceVector();
            Point carPosition = new Point((int)CarShape.MassCentre[0], (int)CarShape.MassCentre[1]);

            int radiusDivisons = rangeFinder._radiusDivisons;

            for (int i = 0; i < sensorsPoints.Count; i++)
            {
                Point measurmentPoint = new Point((int)(carPosition.X + (measurments[i] / radiusDivisons) * sensorsPoints[i][0]),
                    (int)(carPosition.Y + (measurments[i] / radiusDivisons) * sensorsPoints[i][1]));

                measurmentsToDraw.Add(measurmentPoint);
            }

            return measurmentsToDraw;
        }

        public string getHPString()
        {
            return HP + " HP";
        }
    }
}