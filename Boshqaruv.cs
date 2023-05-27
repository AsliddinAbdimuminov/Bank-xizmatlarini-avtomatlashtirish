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
    public partial class Boshqaruv : Form
    {
        public Boshqaruv()
        {
            InitializeComponent();
        }
        public SqlConnection con;
        private void Boshqaruv_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["karta"].ConnectionString);
            wrap();
            dffs("Tarix");

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand($"select * from Transferlar ", con);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                guna2DataGridView2.DataSource = table;
                
                SqlCommand command2 = new SqlCommand($"select sum(summa) from Tarix ", con);
                SqlDataAdapter adapter2 = new SqlDataAdapter(command2);
                DataTable table2 = new DataTable();
                adapter.Fill(table2);
                double jami = double.Parse(table2.Rows[0][0].ToString());
                MessageBox.Show(jami.ToString());
                SqlCommand command3 = new SqlCommand($"select sum(summa) from Tarix ", con);
                SqlDataAdapter adapter3 = new SqlDataAdapter(command3);
                DataTable table3 = new DataTable();
                adapter.Fill(table2);
                jami = jami + double.Parse(table3.Rows[0][0].ToString());
                label16.Text = jami.ToString();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

       public void wrap()
        {
            if (con.State!=ConnectionState.Open)
            {
                con.Open();
            }
            SqlCommand command = new SqlCommand("select count(*) from ka", con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            label13.Text = table.Rows[0][0].ToString();

            command = new SqlCommand("select count(*) from Tarix", con);
            adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            label14.Text = table.Rows[1][0].ToString();

            command = new SqlCommand("select count(*) from Transferlar", con);
            adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            label15.Text = table.Rows[2][0].ToString();
            label16.Text = "";
            con.Close();
        }
        private void guna2Button1_Click_1(object sender, EventArgs e)
        {

            if (guna2TextBox1.Text!="")
            {
                Tekshirish();
            }
            else
            {
                MessageBox.Show("Iltimos ma`lumot kiriting!", "Xabar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Tekshirish()
        {
            dataGridView1.DataSource = null;
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                string dd = String.Empty;
                string[] a = guna2TextBox1.Text.Split(' ');
               
                switch (guna2ComboBox1.Text)
                {
                    case "Id": dd = $"rr={a[0]}"; break;
                    case "Raqami": dd = $"raqam='{a[0]}'"; break;
                    case "Ismi va familyasi": dd = $"ism='{a[0]}' and fam='{a[1]}'"; break;
                    default: break;
                }
                string surov = "select * from ka where "+dd;
                SqlCommand command = new SqlCommand(surov, con);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable set = new DataTable();
                adapter.Fill(set);
                dataGridView1.DataSource = set;
                if (set.Columns.Count > 0)
                {

                    DataRow row = set.Rows[0];
                    label2.Text = row[5].ToString()+" so`m";
                    guna2TextBox1.Clear();
                }
                else
                {
                    MessageBox.Show($"{guna2ComboBox1.Text} dagi foydalanuvchi topilmadi!", "Xabar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    guna2TextBox1.Clear();
                }
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            
            if (guna2TextBox2.Text != "" && guna2TextBox3.Text != "" && double.Parse(guna2TextBox3.Text)>0)
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                string surov = $"select * from ka where rr='{int.Parse(guna2TextBox2.Text)}'";
                SqlCommand command = new SqlCommand(surov, con);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable set = new DataTable();
                adapter.Fill(set);
                if (set.Columns.Count > 0)
                {
                dataGridView1.DataSource = set;

                    DataRow row = set.Rows[0];
                    double pul2 = double.Parse(row[5].ToString());
                    double qiymat = double.Parse(guna2TextBox3.Text);
                    if (qiymat<=pul2)
                    {
                        surov = $"update ka set balans='{pul2 - qiymat}' where rr='{guna2TextBox2.Text}'";
                        SqlCommand command2 = new SqlCommand(surov,con);
                        command2.ExecuteNonQuery();
                        MessageBox.Show($"Mablag` {qiymat} so`m qaytarildi");
                       
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }
                        try
                        {
                            SqlCommand c5 = new SqlCommand($"insert into Transferlar(kimdan, kimga, jarayon, summa, jarayon_vaqti) values('{guna2TextBox2.Text}', 'no', 'Deposit qaytarish', '{double.Parse(guna2TextBox3.Text)}', '{DateTime.Now.ToString("dd-MM-yyyy H:mm:ss")}')", con);
                            c5.ExecuteNonQuery();
                            guna2TextBox3.Clear();
                            guna2TextBox2.Clear();
                            wrap();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"{guna2ComboBox1.Text} dagi foydalanuvchi topilmadi!", "Xabar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                con.Close();
            }
            else if (double.Parse(guna2TextBox5.Text) < 0)
            {
                MessageBox.Show("Boshqa mablag` kiriting chunki bu Deposit qo`shish qilsangiz bo`ladi!", "Xabar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (double.Parse(guna2TextBox5.Text) == 0)
            {
                MessageBox.Show("Bunday mablag` qaytarib ham qo`shib ham bo`lmaydi!", "Xabar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Iltimos ma`lumot kiriting!", "Xabar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text != "" && guna2TextBox5.Text != "" && double.Parse(guna2TextBox5.Text)>0)
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                tek2();

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand($"insert into Transferlar(kimdan, kimga, jarayon, summa, jarayon_vaqti) values('{guna2TextBox4.Text}', 'no', 'Deposit o`tkazish', {double.Parse(guna2TextBox5.Text)}, '{DateTime.Now.ToString("dd-MM-yyyy H:mm:ss")}')", con);
                command.ExecuteNonQuery();
                con.Close();
                wrap();
            }
            else if(double.Parse(guna2TextBox5.Text) < 0)
            {
                MessageBox.Show("Boshqa mablag` kiriting chunki bu Deposit qaytarishda qilsangiz bo`ladi!", "Xabar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (double.Parse(guna2TextBox5.Text) == 0)
            {
                MessageBox.Show("Bunday mablag` qaytarib ham qo`shib ham bo`lmaydi!", "Xabar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Iltimos ma`lumot kiriting!", "Xabar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void tek2()
        {
            
            string surov = $"select * from ka where rr='{int.Parse(guna2TextBox4.Text)}'";
            SqlCommand command = new SqlCommand(surov, con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable set = new DataTable();
            adapter.Fill(set);
            if (set.Columns.Count > 0)
            {
                dataGridView1.DataSource = set;

               
                foreach (DataRow row in set.Rows)
                {
                    double pul2 = double.Parse(row[5].ToString());
                    double qiymat = double.Parse(guna2TextBox5.Text);

                    double sum = pul2 + qiymat;

                    string surov2 = $"update ka set balans='{sum}' where rr='{guna2TextBox4.Text}'";
                    SqlCommand command2 = new SqlCommand(surov2, con);
                    command2.ExecuteNonQuery();

                    MessageBox.Show($"Mablag` {qiymat} so`m qo`shildi");
                    wrap();
                }


            }
            else
            {
                MessageBox.Show($"{guna2TextBox4.Text} dagi foydalanuvchi topilmadi!", "Xabar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

       
        public void displey(string sv, int sel)
        {
            
            try
            {
                
                string ss = $"select * from ka where rr='{sv}'";
                SqlCommand command = new SqlCommand(ss, con);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                if (table.Rows.Count>0)
                {
                    if (sel == 5)
                    {
                        label4.Text = table.Rows[0][sel].ToString();
                    }
                    else label5.Text = table.Rows[0][sel].ToString();
                    displey2.DataSource = table;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);   
            }
        }

        private void kim_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    displey(kim.Text,5);
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void qaysi_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    displey(qaysi.Text,3);
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void uzgarish()
        {
            try
            {
                if (kim.Text != "" && qaysi.Text != "" && pulqiy.Text != "" && kim.Text!=qaysi.Text)
                {
                    string surov = "select * from ka";
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    double ki = 0, qay = 0, wpul = double.Parse(pulqiy.Text);
                    SqlCommand command = new SqlCommand(surov, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[7].ToString() == kim.Text)
                        {
                            ki = double.Parse(row[5].ToString());
                        }
                        if (row[7].ToString() == qaysi.Text)
                        {
                            qay = double.Parse(row[5].ToString());

                        }
                    }
                    if (ki > wpul && wpul > 0)
                    {

                        surov = $"update ka set balans='{ki - wpul}' where rr='{kim.Text}'";
                        SqlCommand command1 = new SqlCommand(surov,con);
                        command1.ExecuteNonQuery();
                        command1 = new SqlCommand($"update ka set balans='{qay + wpul}' where rr='{qaysi.Text}'", con);
                        command1.ExecuteNonQuery();
                        MessageBox.Show($"{kim.Text} dan {qaysi.Text} ga {wpul} so`m transfer qilindi", "Bildirish");
                        wrap();
                    }
                    else if (wpul <= 0)
                    {
                        MessageBox.Show("Mablag`ni to`gri kiriting mavlag`ingiz kam");
                    }
                }
                else if(kim.Text == qaysi.Text)
                {
                    MessageBox.Show("O`ziga-o`zi transfer qilib bo`lmaydi ", "Xabar");
                    
                }
                else
                {
                    MessageBox.Show("Bo`sh qolgan maydonlarni to`ldiring", "Xabar");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Noto`g`ri qiymat kiritdingiz to`g`irlang", "Ogohlantirish");
            }
            
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            uzgarish();
            if (con.State!=ConnectionState.Open)
            {
                con.Open();
            }
            SqlCommand command = new SqlCommand($"insert into Transferlar(kimdan, kimga, jarayon, summa, jarayon_vaqti) values('{kim.Text}', '{qaysi.Text}', 'transfer', '{pulqiy.Text}', '{DateTime.Now.ToString("dd-MM-yyyy H:mm:ss")}')", con);
            command.ExecuteNonQuery();
            label4.Text = "Mablag`i";
            label5.Text = "Ismi";
            kim.Clear();
            qaysi.Clear();
            pulqiy.Clear();
            wrap();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            string date = (DateTime.Now.Month>9)? DateTime.Now.Month.ToString():("0" + DateTime.Now.Month) + "/" + (DateTime.Now.Year%100 + 3).ToString();
            MessageBox.Show(date.ToString());
            if (guna2TextBox6.Text.Length == 16 && tektnum(guna2TextBox6.Text))
            {

                if (guna2TextBox8.Text!="" && guna2TextBox9.Text != "" && guna2TextBox10.Text != "" && guna2TextBox11.Text != "" && guna2TextBox10.Text.Length == 4 && tektnum(guna2TextBox10.Text) && guna2TextBox11.Text.Length==4)
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand command = new SqlCommand($"select * from ka where raqam='{guna2TextBox6.Text}'", con);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count > 0)
                        MessageBox.Show("Bunday raqamli plastik karta egasi bazada mavjud", "Ogohlantirish");
                    else
                    {
                        try
                        {
                            string surov = $"insert into ka(raqam, muddat, ism, fam, balans, parol, rr) " +
                                $"values('{guna2TextBox6.Text}', '{date}', '{guna2TextBox8.Text}', '{guna2TextBox9.Text}',0,'{guna2TextBox10.Text}', '{guna2TextBox11.Text}')";
                            SqlCommand command2 = new SqlCommand(surov, con);
                            command2.ExecuteNonQuery();
                            MessageBox.Show("Karta tayyor", "Xabar");
                            guna2TextBox6.Clear();
                            guna2TextBox8.Clear();
                            guna2TextBox9.Clear();
                            guna2TextBox10.Clear();
                            guna2TextBox11.Clear();
                            con.Close();
                            wrap();
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("To`ldirishda kamchilik bor", "Xabar");
                }
            }
            else MessageBox.Show("Karta raqamini to`g`ri emas 16 ta raqam va harif bo`lmasligi kerak", "Xabar");
        }
        private bool tektnum(string a)
        {
            bool holat = false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i]>47 && 58>a[i])
                {
                    holat = true;
                }
                else
                {
                    holat = false;
                    i = a.Length;
                }
            }
            return holat;
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox14_TextChanged(object sender, EventArgs e)
        {
            dffs($"Tarix where {guna2ComboBox2.Text} like '%{guna2TextBox14.Text}%'");
        }
        private void dffs(string s)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand($"select * from {s}", con);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                guna2DataGridView1.DataSource = table;
                con.Close();
            }
            catch (Exception)
            {

                
            }
        }

        private void guna2TextBox15_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand($"select * from Transferlar where {guna2ComboBox3.Text} like '%{guna2TextBox15.Text}%'", con);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                guna2DataGridView2.DataSource = table;
                con.Close();
            }
            catch (Exception)
            {


            }
        }
    }
}
