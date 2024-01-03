using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using System.Xml.Linq;
using System.Configuration;
using System.Data.SqlClient;

namespace HKTS_Management_Center
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MySqlConnection con, conn;

        string host = "localhost";
        string user = "root";
        string pwd = "root";

        private void button2_Click(object sender, EventArgs e)
        {
            if(kadi.Text != "" && sifre.Text != "")
            {
                con.Open();
                MySqlDataReader dr;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "select * from hesaplar where username='" + kadi.Text + "'";
                dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    con.Close();
                    con.Open();
                    cmd.CommandText = "select * from hesaplar where email='" + email.Text + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.Read())
                    {
                        con.Close();
                        con.Open();
                        cmd.CommandText = "select * from hesaplar where phone='" + telefon.Text + "'";
                        dr = cmd.ExecuteReader();
                        if (!dr.Read())
                        {
                            con.Close();
                            con.Open();
                            cmd.Connection = con;
                            cmd.CommandText = "INSERT INTO hesaplar(username,password,email,phone,regdate,outdate) VALUES(@username,@password,@email,@phone,@regdate,@outdate)";
                            cmd.Parameters.AddWithValue("@username", kadi.Text);
                            cmd.Parameters.AddWithValue("@password", sifre.Text);
                            cmd.Parameters.AddWithValue("@email", email.Text);
                            cmd.Parameters.AddWithValue("@phone", telefon.Text);
                            cmd.Parameters.AddWithValue("@regdate", DateTime.Now.Date);
                            cmd.Parameters.AddWithValue("@outdate", DateTime.Now.Date.AddDays(31));
                            cmd.ExecuteNonQuery();
                            con.Close();
                            hesap();
                            kadi.Text = "";
                            sifre.Text = "";
                            email.Text = "";
                            telefon.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Telefon numarası kullanılıyor!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("E-Posta kullanılıyor!");
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı kullanılıyor!");
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı adı ve şifre zorunlu!");
            }
            con.Close();
        }

        private void hesap()
        {
            con.Open();
            string komut = "select * from hesaplar";
            MySqlDataAdapter da = new MySqlDataAdapter(komut, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
            dataGridView1.AutoResizeColumns();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            string query = "UPDATE ayarlar SET ver=@ver,baslik=@baslik, govde=@govde where id='" + 1 + "'";
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@ver", ver.Text);
            cmd.Parameters.AddWithValue("@baslik", baslik.Text);
            cmd.Parameters.AddWithValue("@govde", govde.Text);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Kaydedildi!");
        }

        string checkver, checkbaslik, checkgovde, id;
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    id = row.Cells[0].Value.ToString();
                }
                MySqlCommand cmd = new MySqlCommand("DELETE  from hesaplar where id='" + id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                hesap();
            }
            else
            {
                MessageBox.Show("Veri seçilmedi!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection("server=" + host + ";user=" + user + ";password=" + pwd);
            con = new MySqlConnection("server=" + host + ";user=" + user + ";password=" + pwd + ";database=HKTS");
            try
            {
                using (conn)
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command = conn.CreateCommand();
                    command.CommandText = "create schema if not exists HKTS";
                    command.ExecuteNonQuery();
                    using (con)
                    {
                        con.Open();
                        MySqlCommand Create_table;
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Ayarlar (id INT NOT NULL AUTO_INCREMENT, ver VARCHAR(60), baslik VARCHAR(100), govde VARCHAR(2500), PRIMARY KEY (id))", con);
                        Create_table.ExecuteNonQuery();
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Hesaplar (id INT NOT NULL AUTO_INCREMENT, username VARCHAR(50), email VARCHAR(200), phone VARCHAR(12), password VARCHAR(200), regdate DATETIME(6), outdate DATETIME(6), PRIMARY KEY (id))", con);
                        Create_table.ExecuteNonQuery();
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Hastalar (ID INT NOT NULL AUTO_INCREMENT, Adult VARCHAR(50), TC_NO VARCHAR(12), AD VARCHAR(41), SOYAD VARCHAR(41), TELEFON VARCHAR(12), CINSIYET VARCHAR(6), DOGUM_TARIHI DATETIME(6), KAYIT_TARIHI DATETIME(6), PRIMARY KEY (ID))", con);
                        Create_table.ExecuteNonQuery();
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Doktorlar (ID INT NOT NULL AUTO_INCREMENT, Adult VARCHAR(50), TC_NO VARCHAR(12), AD VARCHAR(41), SOYAD VARCHAR(41), TELEFON VARCHAR(12), ALAN VARCHAR(100), CINSIYET VARCHAR(6), DOGUM_TARIHI DATETIME(6), KAYIT_TARIHI DATETIME(6), PRIMARY KEY (ID))", con);
                        Create_table.ExecuteNonQuery();
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Randevular (ID INT NOT NULL AUTO_INCREMENT, Adult VARCHAR(50), TC_NO VARCHAR(12), AD VARCHAR(41), SOYAD VARCHAR(41), DOKTOR VARCHAR(100), TARIH DATETIME(6), TUR VARCHAR(50), PRIMARY KEY (ID))", con);
                        Create_table.ExecuteNonQuery();
                        Create_table = new MySqlCommand("CREATE TABLE if not exists Ameliyatlar (ID INT NOT NULL AUTO_INCREMENT, Adult VARCHAR(50), TC_NO VARCHAR(12), AD VARCHAR(41), SOYAD VARCHAR(41), DOKTOR VARCHAR(100), TARIH DATETIME(6), TUR VARCHAR(50), AMELIYAT VARCHAR(200), PRIMARY KEY (ID))", con);
                        Create_table.ExecuteNonQuery();
                        con.Close();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri tabanına bağlanılamadı!\n\n" + ex.Message);
            }
            finally
            {
                con.Close();
                conn.Close();
            }
            con.Open();
            MySqlCommand cmd = new MySqlCommand("Select * from ayarlar", con);
            MySqlDataReader dr = cmd.ExecuteReader();
            if(!dr.HasRows)
            {
                con.Close();
                con.Open();
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO ayarlar(ver,baslik,govde) VALUES(@ver,@baslik,@govde)";
                cmd.Parameters.AddWithValue("@ver", "");
                cmd.Parameters.AddWithValue("@baslik", "");
                cmd.Parameters.AddWithValue("@govde", "");
                cmd.ExecuteNonQuery();
                con.Close();
            }
            con.Close();
            con.Open();
            string sorgu = "SELECT * FROM Ayarlar where id='" + 1 + "'";
            cmd = new MySqlCommand(sorgu, con);
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr1 in dt.Rows)
            {
                checkver = dr1["ver"].ToString();
                checkbaslik = dr1["baslik"].ToString();
                checkgovde = dr1["govde"].ToString();
            }
            con.Close();
            ver.Text = checkver;
            baslik.Text = checkbaslik;
            govde.Text = checkgovde;
            hesap();

        }
    }
}
