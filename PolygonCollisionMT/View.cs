using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PolygonCollisionMT
{
    public partial class View : UserControl
    {
        Graphics _canvas;
        PolygonManager _polygonManager;
        Road _road;
        Bitmap _bmp;
        Car _car;

        bool readyToDraw;
        Simulation _simulation;


        public View()
        {
            readyToDraw = false;
            InitializeComponent();
            _canvas = panel1.CreateGraphics();
            timer1.Enabled = true;
            _bmp = new Bitmap(panel1.Width, panel1.Height);

        }

        public void setPolygonManager(PolygonManager pm)
        {
            _polygonManager = pm;
        }

        public void setRoad(Road road)
        {
            _road = road;
        }

        public void setCar(Car car)
        {
            _car = car;
        }

        public void setSimulation(Simulation simulation)
        {
            disableDrawing();
            _simulation = simulation;
            setPolygonManager(_simulation.getPolygonManager());
            setRoad(_simulation.getRoad());
            setCar(_simulation.getCar());

            enableDrawing();

            drawScreen();
            pictureBox1.Image = _bmp;
            pictureBox1.Update();
            panel1.Invalidate();

            //disableDrawing();

        }

        private void drawScreen()
        {
            if (readyToDraw)
            {
                using (Graphics g = Graphics.FromImage(_bmp))
                {

                    drawPolygons(g);
                    drawHP(g);
                    drawScore(g);
                    drawSensors(g);
                    drawMeasurments(g);
                    drawTime(g);
                }
            }
        }

        private void drawPolygons(Graphics g)
        {
                Brush brush = new SolidBrush(Color.Black);
                Rectangle area = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
                g.FillRectangle(brush, area);
                List<Point[]> polygonsToDraw = _polygonManager.getPolygonsPointsList();
                

                foreach (Point[] p in polygonsToDraw)
                {
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i].Y = p[i].Y - _road.VisibleArea.CurrentAreaPosition;// + 400;
                    }
                    g.DrawLines(new Pen(Color.LimeGreen), p);
                }
        }

        private void drawHP(Graphics g)
        {
            Brush brush = new SolidBrush(Color.LimeGreen);
            Font font = new Font("Arial", 12);
            Point point = new Point(40,40);
            string hp = _car.getHPString();
            g.DrawString(hp, font, brush, point);
        }

        private void drawScore(Graphics g)
        {
            Brush brush = new SolidBrush(Color.LimeGreen);
            Font font = new Font("Arial", 12);
            Point point = new Point(40, 70);
            string score = _car.getDistanceScore() + " Score";
            g.DrawString(score, font, brush, point);
        }

        private void drawSensors(Graphics g)
        {
            List<List<Point>> sensors = _car.sensorsToDraw();
            Pen pen = new Pen(Color.Aqua);

            for (int i = 0; i < sensors.Count; i++)
            {
                Point pointToDraw1 = new Point(sensors[i][0].X, sensors[i][0].Y- _road.VisibleArea.CurrentAreaPosition);
                Point pointToDraw2 = new Point(sensors[i][1].X, sensors[i][1].Y - _road.VisibleArea.CurrentAreaPosition);

                g.DrawLine(pen, pointToDraw1, pointToDraw2);
            }
        }

        private void drawTime(Graphics g)
        {
            Brush brush = new SolidBrush(Color.LimeGreen);
            Font font = new Font("Arial", 12);
            Point point = new Point(40, 100);

            long time = _simulation.getTime();

            string timeString = time + " s.";
            g.DrawString(timeString, font, brush, point);
        }

        private void drawMeasurments(Graphics g)
        {
            List<Point> measurments = _car.measurmentsToDraw();
            Pen pen = new Pen(Color.Cyan);

            for (int i = 0; i < measurments.Count; i++)
            {
                Point point1 = new Point(measurments[i].X-3, measurments[i].Y - _road.VisibleArea.CurrentAreaPosition-3);
                Size size = new Size(6, 6);
                Rectangle rectangleToDraw = new Rectangle(point1,size);

               g.DrawEllipse(pen, rectangleToDraw);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if(readyToDraw && !_simulation.isFinished())
            {
                _road.VisibleArea.CurrentAreaPosition = (int)_polygonManager.carPolygon.MassCentre[1] - 100;//(int)_road.Car.carShape.MassCentre[0];
                drawScreen();    
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!readyToDraw)
            {
                return;
            }
                pictureBox1.Image = _bmp;
                pictureBox1.Update();
                panel1.Invalidate();
        }

        public void enableDrawing()
        {
            readyToDraw = true;
        }

        public void disableDrawing()
        {
            readyToDraw = false;
        }
    }
}
