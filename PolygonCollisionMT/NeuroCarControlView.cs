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
    public partial class NeuroCarControlView : UserControl
    {
        

        NeuroCarControl neuroControl;

        public NeuroCarControlView()
        {
            InitializeComponent();

            neuroControl = new NeuroCarControl();

        }
        public void setCar(Car car)
        {
            neuroControl.setCar(car);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            neuroControl.setControl((int)numericUpDown1.Value, (int)numericUpDown2.Value,
                (int)numericUpDown3.Value, (double)numericUpDown4.Value);
        }

    }
}
