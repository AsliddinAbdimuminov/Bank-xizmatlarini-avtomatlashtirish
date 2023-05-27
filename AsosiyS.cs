using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using System.Data.SqlClient;
using System.Configuration;

namespace Bank29
{
    public partial class AsosiyS : Form
    {
        public AsosiyS()
        {
            InitializeComponent();
        }
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice captureDevice;
        public SqlConnection con = null;
        public string suz2 = string.Empty;
        private void AsosiyS_Load(object sender, EventArgs e)
        {
            
            
            try
            {
                filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo filterInfo in filterInfoCollection)
                {
                    comboBox1.Items.Add(filterInfo.Name);
                }
                comboBox1.SelectedIndex = 0;
                captureDevice = new VideoCaptureDevice(filterInfoCollection[comboBox1.SelectedIndex].MonikerString);
                captureDevice.NewFrame += CaptureDevice_NewDrame;
                captureDevice.Start();
                timer1.Start();
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["karta"].ConnectionString);
                con.Open();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void CaptureDevice_NewDrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void AsosiyS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (captureDevice.IsRunning)
            {
                captureDevice.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Image!=null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)pictureBox1.Image);
                if (result!=null && result.Text.Length==19)
                {
                    guna2TextBox1.Text = result.ToString();
                    string suz = result.ToString();
                    if (guna2TextBox1.Text.Length == 19)
                    {            
                        if (tekshirish(con, suz))
                        {
                            panel1.Visible = true;
                            tbpin.Select();
                        }
                        else MessageBox.Show("Bu plastik noqonuniy");
                    }
                }
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string krqama = guna2TextBox1.Text;
            string kraqam = string.Empty;
            if (krqama.Length==16)
            {
                for (int i = 0; i < 16; i++)
                {
                    if (i%4==0 && i!=15 && i!=0)
                    {
                        kraqam += " ";
                    }
                    kraqam += krqama[i];
                }
                guna2TextBox1.Text = kraqam;
                suz2 = kraqam;
                if (tekshirish(con, kraqam))
                {
                    panel1.Visible = true;
                    tbpin.Select();
                }
                else
                {
                    MessageBox.Show("Bunday raqamli karta mavjud emas", "Xabarnoma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    guna2TextBox1.Text = "";
                }
            }
        }

        private void tbpin_TextChanged(object sender, EventArgs e)
        {
            if (tbpin.TextLength==4)
            {
                string suz = "";
                foreach (var item in suz2.ToString())
                    if (item != ' ')
                        suz += item;
                SqlCommand command2 = new SqlCommand(
                    $"select raqam, parol from ka where raqam='{suz}' and parol='{tbpin.Text}'",con);
                SqlDataAdapter adapter = new SqlDataAdapter(command2);
                DataTable table = new DataTable();
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    captureDevice.Stop();
                    timer1.Stop();
                    this.Hide();
                    tankarta tankarta = new tankarta();
                    tankarta.suz2=suz2;
                    tankarta.ShowDialog();
                    
                    this.Close();
                }
                else MessageBox.Show("Topilmadi");
            }
        }


        private void guna2CircleButton9_Click(object sender, EventArgs e)
        {
            try
            {
                Guna.UI2.WinForms.Guna2CircleButton btn = (Guna.UI2.WinForms.Guna2CircleButton)sender;
                string text = btn.Text;
                tbpin.Text += text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void guna2CircleButton11_Click(object sender, EventArgs e)
        {
            if (tbpin.Text.Length!=0)
                tbpin.Text = tbpin.Text.Substring(0, tbpin.Text.Length - 1);
        }

        private void guna2CircleButton12_Click(object sender, EventArgs e)
        {
            tbpin.Clear();
        }
        public bool tekshirish(SqlConnection con1, string result)
        {
            string suz = "";
            foreach (var item in result.ToString())
                if (item != ' ')
                    suz += item;
            suz2 = suz;

            string surov = $"select raqam from ka where raqam='{suz}'";
            SqlCommand command = new SqlCommand(surov, con1);
            SqlDataAdapter sqlDataA = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            sqlDataA.Fill(dataTable);
            int a=dataTable.Rows.Count;
            return (a > 0) ? true : false;
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            captureDevice.Stop();
            timer1.Stop();

            tanlash tanlash = new tanlash();
            this.Hide();

            tanlash.ShowDialog();
            this.Close();
        }
    }
}
