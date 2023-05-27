using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCoder;
using System.Configuration;
using System.Data.SqlClient;
using Guna.UI2;
using Guna.UI2.WinForms;

namespace Bank29
{
    public partial class tankarta : Form
    {
        public tankarta()
        {
            InitializeComponent();
        }
        
        kutish kutish = new kutish();
        Funksiyalar.Baza_uchun baza = new Funksiyalar.Baza_uchun();
        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            kutish.ShowDialog();
            this.Close();
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["karta"].ConnectionString);
        private void tankarta_Load(object sender, EventArgs e)
        {
            try
            {
                QRCodeGenerator qRCode = new QRCodeGenerator();
                QRCodeData codeData = qRCode.CreateQrCode(suz2, QRCodeGenerator.ECCLevel.Q);
                QRCode qR = new QRCode(codeData);
                pictureBox1.Image = qR.GetGraphic(5);
            }
            catch (Exception) { }
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.Blue;
            button1.ForeColor = Color.White;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.OrangeRed;
            button1.ForeColor = Color.Blue;
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            string surov = $"delete ka where raqam='{suz2}'";
            baza.uchirish1(surov);
            MessageBox.Show("Plastik kartangiz bazadan o`chdi", "Ma`lumot", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            kutish.ShowDialog();
            this.Hide();
            this.Close();
        }
        AsosiyS asosiy = new AsosiyS();
        object balans=null;
        private void guna2Button9_Click(object sender, EventArgs e)
        {
            try
            {
                string surov = $"select balans from ka where raqam={suz2}";

                SqlDataReader dataReader = baza.uqishSDR(surov);

                while (dataReader.Read())
                {
                    balans = dataReader["balans"];
                }
                MessageBox.Show("Sizda mavjud summa " + balans.ToString() + " so`m", "Balans", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataReader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                
                
            }
        }

        public string suz2;

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            try
            {
                string surov = $"update ka set parol='{guna2TextBox1.Text}' where raqam='{suz2}'";
                baza.uzgarish1(surov);
                MessageBox.Show("Parolingiz muvaffaqaqiyatli yangilandi", "O`zarishi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                guna2TextBox1.Text = "";
            }
            catch (Exception ex)
            {

                
            }
        }

        public void Displey()
        {
            try
            {
                if(con.State!=ConnectionState.Open)
                    con.Open();
                SqlCommand command3 = new SqlCommand(
                    "select * from Tarix", con
                    );
                SqlDataAdapter adapter = new SqlDataAdapter(command3);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                con.Close();
            }
            catch (Exception)
            {

               
            }
        }
        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Displey();
        }
        Pulyechish pulyechish = new Pulyechish();
        private void guna2Button8_Click(object sender, EventArgs e)
        {
            
            pulyechish.raqam = suz2;
            pulyechish.ShowDialog();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            pulyechish.raqam = suz2;
            Guna2Button son = (Guna2Button)sender;
            string[] s = son.Text.Split(' ');
            pulyechish.pul2 = s[0] +s[1];
            pulyechish.ShowDialog();
        }

        public string printText()
        {
            if (con.State!=ConnectionState.Open)
            {
                con.Open();
            }
            SqlCommand command = new SqlCommand(
                "select * from Tarix",con
                );
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable set = new DataTable();
            adapter.Fill(set);
            string su = "--------------------------------------------------------------------------------------------------\n";
            foreach (DataRow r in set.Rows)
            {
                su += "| "+r[0]+" | "+r[1]+" | "+r[2]+" | "+r[3]+" | "+r[4]+" |\n";
                su += "--------------------------------------------------------------------------------------------------\n";
            }
            return su;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 15, FontStyle.Regular);
            Brush brush = Brushes.Black;
            PointF point = new PointF(10, 10);
            e.Graphics.DrawString(printText(), font, brush, point);
        }
        private void guna2Button12_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.Height = this.Height;
            printPreviewDialog1.Width = this.Width;
            printPreviewDialog1.ShowDialog();
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            printPreviewDialog2.Document = printDocument2;
            printPreviewDialog2.Height = this.Height;
            printPreviewDialog2.Width = this.Width;
            printPreviewDialog2.ShowDialog();
        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            string surov = $"select balans from ka where raqam={suz2}";

            SqlDataReader dataReader = baza.uqishSDR(surov);

            while (dataReader.Read())
            {
                balans = dataReader["balans"];
            }
            dataReader.Close();
            con.Close();
            Font font = new Font("Arial", 20, FontStyle.Bold);
            Brush brush = Brushes.Black;
            PointF point = new PointF(100, 100);
            e.Graphics.DrawString(naxtPrint(balans.ToString()), font, brush, point);
        }
        public string naxtPrint(string ss)
        {
            DateTime vaqt = DateTime.Now;
            string rr = "Karta raqam: " + suz2.ToString() + "" +
                "\n Xizmat turi: Tekshirish\n\n" +
                $"Summa: {ss} so`m\n\n" +
                $"Ish bajarilgan vaqt: {vaqt.ToString()}";
            return rr;
        }

        private void printDocument3_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox1.Image, 350, 250);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printPreviewDialog3.Height = this.Height;
            printPreviewDialog3.Width = this.Width;
            printPreviewDialog3.ShowDialog();

        }
    }
}
