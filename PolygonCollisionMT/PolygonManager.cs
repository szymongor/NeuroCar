﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace PolygonCollisionMT
{
    public class PolygonManager
    {
        private List<Polygon> _polygons;
        private List<Polygon> _polygonsBeforeMove;
        private int _boundaryX;
        private int _boundaryY;
        private Thread PolygonManagerThread;
        bool _isThreadRunning;
        bool _stopThread;
        long _threadTime;
        private bool running;
        public Polygon carPolygon;
        public Polygon carPolygonBeforeMove;
        private Car _car;
        private Road _road;
        //
        string _name = "temp";
        public MyVector lastForce;
        //
        public PolygonManager(int boundaryX, int boundaryY, Car car)
        {
            _polygons = new List<Polygon>();
            _boundaryX = boundaryX;
            _boundaryY = boundaryY;
            _isThreadRunning = false;
            startThread();
            running = true;
            //
            carPolygon = car.CarShape;
            _car = car;
            lastForce = new MyVector(0,0);
            //
        }

        public PolygonManager(int boundaryX, int boundaryY, Car car, string name)
        {
            _name = name;
            _polygons = new List<Polygon>();
            _boundaryX = boundaryX;
            _boundaryY = boundaryY;
            _isThreadRunning = false;
            startThread();
            running = true;
            //
            carPolygon = car.CarShape;
            _car = car;
            lastForce = new MyVector(0, 0);
            //
        }

        public void threadAction(object data)
        {
            while (!_stopThread)
            {

                if (running)
                {
                    if (_road != null)
                    {
                        setPolygonList(_road.getVisiblePolygons());
                    }
                    movePolygons();
                }
                Thread.Sleep(10);
            }
            //PolygonManagerThread.Abort();
            return;
        }

        public void startThread()
        {
            _threadTime = DateTime.Now.Ticks;
            string date = DateTime.Now.Minute.ToString();
            PolygonManagerThread = new Thread(this.threadAction);
            PolygonManagerThread.Start();
            PolygonManagerThread.Name = "PolygonManagerThread" + date;
            _isThreadRunning = true;
            _stopThread = false;

        }

        public void stopThread()
        {
            //PolygonManagerThread.Abort();
            _isThreadRunning = false;
            _stopThread = true;
            //PolygonManagerThread = null;
        }

        public void addPolygon(Polygon p)
        {
            while (!running)
            {

            }
            running = false;
            _polygons.Add(p);
            running = true;
        }

        public void setRoad(Road road)
        {
            _road = road;
        }

        public void setPolygonList(List<Polygon> polygons)
        {
            while (!running)
            {

            }
            running = false;
            _polygons = polygons;
            running = true;
        }

        private Polygon getNextPolygon()
        {
            while (!running && _isThreadRunning)
            {

            }
            running = false;
            Polygon p;
            try
            {
                p = _polygons[0];
                _polygons.RemoveAt(0);
                _polygons.Add(p);
            }
            catch (ArgumentOutOfRangeException)
            {
                p = null;
            }
            catch (IndexOutOfRangeException)
            {
                p = null;
            }
            catch (ArgumentException)
            {
                p = null;
            }
            running = true;
            return p;
        }

        //public List<Polygon> getPolygonList()
        //{
        //    List<Polygon> returnList = new List<Polygon>();

        //    while (!running)
        //    {

        //    }
        //    running = false;
        //    try
        //    {
        //        foreach (Polygon p in _polygons)
        //        {
        //            returnList.Add(p.getClone());
        //        }
        //    }
        //    catch (InvalidOperationException)
        //    {

        //    }
        //    running = true;
        //    return returnList;

        //}

        public List<Polygon> getPolygonList()
        {
            List<Polygon> returnList = new List<Polygon>();

            for (int i = 0; i < _polygons.Count; i++)
            {
                Polygon p = getNextPolygon();
                try
                {
                    returnList.Add(p.getClone());
                }
                catch (NullReferenceException)
                {

                }
            }                 
            return returnList;

        }

        public List<Polygon> getPolygonBeforeMoveList()
        {
            List<Polygon> returnList = new List<Polygon>();

            running = false;
            foreach (Polygon p in _polygonsBeforeMove)
            {
                returnList.Add(p.getClone());
            }
            running = true;
            return returnList;

        }

        public List<Point[]> getPolygonsPointsList()
        {
            List<Polygon> polygonsClone = getPolygonList();
            List<Point[]> polygonsPointsList = new List<Point[]>();
            if (!_isThreadRunning)
            {
                //int i = 0;
            }
            try
            {
                foreach (Polygon p in polygonsClone)
                {
                    polygonsPointsList.Add(p.getPointsTable());
                }
                polygonsPointsList.Add(carPolygon.getPointsTable());
            }
            catch (InvalidOperationException)
            {
                return polygonsPointsList;
            }
            
            return polygonsPointsList;
        }

        public List<Point[]> getPolygonsBeforeMovePointsList()
        {
            List<Polygon> polygonsClone = getPolygonBeforeMoveList();
            List<Point[]> polygonsPointsList = new List<Point[]>();

            try
            {
                foreach (Polygon p in polygonsClone)
                {
                    polygonsPointsList.Add(p.getPointsTable());
                }
                //polygonsPointsList.Add(carPolygonBeforeMove.getPointsTable());
            }
            catch (InvalidOperationException)
            {
                return polygonsPointsList;
            }

            return polygonsPointsList;
        }

        public void movePolygons()
        {
            
            checkBoundaryCollisions();
            checkObstacleCollison();
            try
            {
                //_polygonsBeforeMove = new List<Polygon>(_polygons);
                _polygonsBeforeMove = getPolygonList();
                
                for (int i = 0; i < _polygons.Count; i++)
                {
                    Polygon p = getNextPolygon();
                    while (!running)
                    {

                    }
                    try
                    {
                        p.shift();
                        p.rotate();
                    }
                    catch (NullReferenceException)
                    {

                    }

                }
                carPolygonBeforeMove = new Triangle(carPolygon);
                _polygonsBeforeMove.Add(carPolygonBeforeMove);
                carPolygon.shift();
                carPolygon.rotate();
            } 
            catch(InvalidOperationException){

            }
            
        }

        public void checkObstacleCollison()
        {
            List<Polygon> polygonsClone = getPolygonList();
            foreach (Polygon p in polygonsClone)
            {
                MyVector contactPoint = carPolygon.polygonCollision(p);
                if (contactPoint[0] != -1)
                {
                    MyVector massCentre = carPolygon.MassCentre - p.MassCentre;
                    MyVector force = carPolygon.getForceVector(contactPoint);
                    Double forceValue = MyMath.normVector(force);
                    force =  MyMath.normalizeVector(force+massCentre) * forceValue;
                    carPolygon.actForce(contactPoint, force);
                    _car.damageCar();
                    //p.actForce(contactPoint, force*-1);
                }
                else
                {
                    MyVector contactPoint2 = p.polygonCollision(carPolygon);
                    if (contactPoint2[0] != -1)
                    {
                        MyVector massCentre = carPolygon.MassCentre - p.MassCentre;
                        MyVector force = carPolygon.getForceVector(contactPoint2);
                        Double forceValue = MyMath.normVector(force);
                        force = MyMath.normalizeVector(force + massCentre) * forceValue;
                        carPolygon.actForce(contactPoint2, force);
                        _car.damageCar();
                        //p.actForce(contactPoint2, force * -1);
                    }
                }
            }
        }

        public void checkBoundaryCollisions()
        {
            try
            {
                MyVector contactPoint = carPolygon.boundaryCollison(0, _boundaryX, 0, _boundaryY);
                //boundary collison zwraca punkt (-1,0) jeśli brak kolizji
                if (contactPoint[0] != -1)
                {
                    //x=0 lub x = max
                    if (contactPoint[0] < 0 || contactPoint[0] > _boundaryX)
                    {
                        MyVector force = carPolygon.getForceVector(contactPoint);
                        force[0] *= -1;
                        carPolygon.actForce(contactPoint, force);
                        lastForce = force;
                        _car.damageCar();
                    }
                    //y=0 lub y = max
                    if (contactPoint[1] < 0 || contactPoint[1] > _boundaryY)
                    {
                        MyVector force = carPolygon.getForceVector(contactPoint);
                        force[1] *= -1;
                        carPolygon.actForce(contactPoint, force);
                        lastForce = force;
                        _car.damageCar();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public bool checkPointBoundaryCollision(MyVector point)
        {
            if (point[0] < 0 || point[0] > _boundaryX)
            {
                return true;
            }
            if (point[1] < 0 || point[0] > _boundaryY)
            {
                return true;
            }
            return false;
        }

    }
}
