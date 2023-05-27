using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Bank29
{
    public partial class Pulyechish : Form
    {
        public Pulyechish()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        double pul;
        double foiz;
        private void pulyech_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (pulyech.Text != null)
                {
                    pul = double.Parse(pulyech.Text);
                    foiz = pul / 100;
                    label3.Text = foiz.ToString() + " so`m";
                    label4.Text = (foiz + pul).ToString() + " so`m";
                }
                else
                {
                    label3.Text = "";
                    label4.Text = "";
                }
            }
            catch (Exception)
            {
                if (pulyech.Text!="")
                {
                    MessageBox.Show("Iltimos faqat raqam ko`rinishda kiriting!", "Bildirish");
                    pulyech.Text = "";
                }
            }
        }
        public string raqam;
        Funksiyalar.Baza_uchun baza = new Funksiyalar.Baza_uchun();
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (pul%10_000==0 && pul<=10_000_000 && pul!=0)
            {
                string s = $"select balans from ka where raqam='{raqam}'";
                SqlDataAdapter reader= baza.uqishSDA(s);
                DataSet set = new DataSet();
                reader.Fill(set);
                double a = double.Parse(set.Tables[0].Rows[0][0].ToString());
                if (a>(pul+foiz))
                {
                    double summa = a - (pul + foiz);
                    string surov = $"update ka set balans='{summa}' where raqam='{raqam}'";
                    baza.uzgarish1(surov);
                    MessageBox.Show($"Sizdan {pul+foiz} so`m pul yechildi", "Xabarnoma");
                    DateTime dateTime = DateTime.Now;
                    surov = $"insert into Tarix(raqam, amal, vaqt, summa) values('{raqam}','Naqqt pul', '{dateTime.ToString("dd/MM/yyyy H:m:ss ")}', '{foiz + pul}' )";
                    baza.malumotQushish(surov);
                    tankarta tankarta = new tankarta();
                    tankarta.Displey();
                    //pechat joyi

                    printPreviewDialog1.Document = printDocument1;
                    printPreviewDialog1.Height = this.Height;
                    printPreviewDialog1.Width = this.Width;
                    printPreviewDialog1.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sizda kerakli mablag` mavjud emas \n" +
                        "Yechib olish summasini qayta kiriting!","Xabarnoma",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Siz juda noto`g`ri summa kiritingiz. \nIltimos qaytadan kiriting", "Ogohlantirish", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pulyech.Text = "";
                label3.Text = "";
                label4.Text = "";
            }
        }
        public string pul2 = "";
        private void Pulyechish_Load(object sender, EventArgs e)
        {
            pulyech.Text = pul2;
            try
            {
                if (pulyech.Text != null)
                {
                    pul = double.Parse(pulyech.Text);
                    foiz = pul / 100;
                    label3.Text = foiz.ToString() + " so`m";
                    label4.Text = (foiz + pul).ToString() + " so`m";
                }
                else
                {
                    label3.Text = "";
                    label4.Text = "";
                }
            }
            catch (Exception)
            {
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 20, FontStyle.Bold);
            Brush brush = Brushes.Black;
            PointF point = new PointF(100, 100);
            e.Graphics.DrawString(naxtPrint(), font, brush, point);
        }
        
        
        public string naxtPrint()
        {
            DateTime vaqt = DateTime.Now;
            string rr = "Karta raqam: " + raqam.ToString() + "" +
                "\n Xizmat turi: Pul yechish\n\n" +
                $"Summa: {foiz+pul} so`m\n\n" +
                $"Ish bajarilgan vaqt: {vaqt.ToString()}";
            return rr;
        }
    }
}
