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
    public partial class SimulationManagerView : UserControl
    {
        SimulationManager _SM;
        View _view;
        double _bestScore;
        double _currentGeneration;
        bool _update;

        public SimulationManagerView()
        {
            InitializeComponent();
            _bestScore = 0;
            _currentGeneration = 0;
            _update = false;
        }

        public void setSimulationManager(SimulationManager SM)
        {
            _SM = SM;
        }

        public void setView(View view)
        {
            _view = view;
        }

        public void setBestScore(double score)
        {
            if (score > _bestScore)
            {
                _bestScore = score;
            }          
        }

        public void nextGeneration(double gen)
        {
            _currentGeneration+=gen;
        }

        public void updateView()
        {
            _update = true;
        }

        private void updateComboBox()
        {
            try
            {
                comboBox1.Items.Clear();
                foreach (Simulation sim in _SM.getSimulationList())
                {
                    comboBox1.Items.Add(sim);
                }
            }
            catch (InvalidOperationException)
            {

            }         
        }

        public void selectComboBox()
        {
            if (comboBox1.Items.Count >0)
            comboBox1.SelectedIndex = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tb_Generation.Text = _currentGeneration.ToString();
            tb_BestScore.Text = _bestScore.ToString();
            progressBar1.Value = (int)MyMath.mapOnRange(_SM.getProgress(),100);
            if (_update)
            {
                updateComboBox();
                selectComboBox();
                _update = false;
            }
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            updateComboBox();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _view.setSimulation((Simulation)comboBox1.SelectedItem);
        }
    }
}
