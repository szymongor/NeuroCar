using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonCollisionMT
{
    public partial class Form1 : Form
    {

        SimulationManager SimulationManager;
        //protected override CreateParams CreateParams // nie miga
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}

        public Form1()
        {
            InitializeComponent();

            SimulationManager = new SimulationManager();

            SimulationManager.setView(simulationManagerView1);
            simulationManagerView1.setSimulationManager(SimulationManager);
            simulationManagerView1.setView(view2);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int boundaryX = view2.Size.Width;
            int roadLength = 3500;

            //for (int i = 0; i < nud_GenerationSize.Value; i++)
            //{
            SimulationManager.startNewSimulations((int)nud_GenerationSize.Value, (int)nud_timeLimit.Value, 
                    boundaryX, roadLength, (int)numericUpDown1.Value,
                    (int)numericUpDown2.Value,
                    (int)numericUpDown3.Value, (double)numericUpDown4.Value);

                //comboBox1.Items.Add(SimulationManager.getSimulation());
            //}

            view2.setSimulation(SimulationManager.getSimulation());
            view2.enableDrawing();

            //comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SimulationManager.stopThreads();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            view2.setSimulation(SimulationManager.getSimulation(comboBox1.SelectedIndex));
            view2.enableDrawing();
        }
    }
}
