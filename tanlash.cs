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
    public partial class tanlash : Form
    {
        public tanlash()
        {
            InitializeComponent();
        }
        public int Navbat;
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AsosiyS asosiyS = new AsosiyS();
            asosiyS.ShowDialog();
            Navbat = 1;
            this.Close();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Navbat = 2;

        }
    }
}
