using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank29
{
    public partial class kutish : Form
    {
        public kutish()
        {
            InitializeComponent();
        }

        private void kutish_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
        int a=0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            guna2CircleProgressBar1.Value = a;
            a++;
            if (a==101)
            {
                this.Hide();
                tanlash tanlash = new tanlash();
                tanlash.ShowDialog();
                timer1.Enabled = false;
                this.Close();
            }
        }
    }
}
