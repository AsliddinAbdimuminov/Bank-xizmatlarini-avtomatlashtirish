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
    public partial class Log_xodim : Form
    {
        public Log_xodim()
        {
            InitializeComponent();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            tanlash tanlash = new tanlash();
            this.Hide();
            tanlash.ShowDialog();
            this.Close();
        }

        private void Log_xodim_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
