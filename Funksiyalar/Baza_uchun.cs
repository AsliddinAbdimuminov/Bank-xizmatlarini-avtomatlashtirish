using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Bank29.Funksiyalar
{
    class Baza_uchun
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["karta"].ConnectionString);
        public void uchirish1(string surov)
        {
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlCommand command = new SqlCommand(surov, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public void uzgarish1(string surov)
        {
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlCommand command2 = new SqlCommand(surov, con);
            command2.ExecuteNonQuery();
            con.Close();
        }
        public void malumotQushish(string surov)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                SqlCommand command2 = new SqlCommand(surov, con);
                command2.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public SqlDataReader uqishSDR(string surov)
        {
            if(con.State!=ConnectionState.Open)
                con.Open();
            SqlCommand command3 = new SqlCommand(surov, con);
            SqlDataReader reader = command3.ExecuteReader();
            return reader;
        }
        public SqlDataAdapter uqishSDA(string surov)
        {
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlCommand command4 = new SqlCommand(surov, con);
            SqlDataAdapter reader = new SqlDataAdapter(command4);
            return reader;
        }
    }
}
