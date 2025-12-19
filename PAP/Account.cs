using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading;
using System.IO;

namespace PAP
{

    public partial class Account : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");

        Thread t1;

        public Account()
        {
            InitializeComponent();
        }

        private void Account_Load(object sender, EventArgs e)
        {
            label3.Text = Home.uid.ToString();

            try
            {
                SqlDataReader reader = null;
                try
                {
                    con.Open();
                    string command = "SELECT name, balance, data_adesao, total_pips, profile_photo, symbol_currency FROM Account, Currency WHERE Account.id_currency = Currency.id_currency AND uid = @uid";

                    SqlParameter param = new SqlParameter();

                    param.ParameterName = "@uid";

                    param.Value = Home.uid;

                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(param);
                    cmd.CommandText = command;

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        label1.Text = (reader.GetString(0));
                        label7.Text = ("Data de Adesão: " + reader.GetDateTime(2).ToString("dd/MM/yyyy"));
                        label9.Text = ("Total Pips: " + reader.GetInt32(3).ToString());
                        label8.Text = ("Moeda: " + reader.GetString(5));

                        double balance = (reader.GetDouble(1));

                        label5.Text = string.Format("{0:#,0.00}", balance) + reader.GetString(5);

                        //GET IMAGE
                        if (reader.GetSqlBytes(4).Buffer != null)
                        {
                            MemoryStream str = new MemoryStream(reader.GetSqlBytes(4).Buffer);
                            circularPictureBox1.Image = Image.FromStream(str);
                        }
                        
                    }
                }
                catch (SqlException s)
                {
                    MessageBox.Show("Erro no Comando SQL: " + s.Message.ToString());
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (con != null)
                    {
                        con.Close();
                    }
                }
            }
            catch (SqlException s)
            {
                MessageBox.Show("Erro no Comando SQL: " + s.Message.ToString());
            }
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            if (gunaButton2.Visible == true)
            {
                gunaButton2.Visible = false;
                gunaButton3.Visible = false;
                gunaButton4.Visible = false;

                gunaButton1.Image = PAP.Properties.Resources.forward;
            }
            else
            {
                gunaButton2.Visible = true;
                gunaButton3.Visible = true;
                gunaButton4.Visible = true;

                gunaButton1.Image = PAP.Properties.Resources.back;
            }
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            t1 = new Thread(abrirjanela_imagem);
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
        }

        private void gunaButton4_Click(object sender, EventArgs e)
        {
            t1 = new Thread(abrirjanela_confirm);
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
            Home.ActiveForm.Close();
        }

        private void abrirjanela_imagem(object obj)
        {
            Application.Run(new Change_Image());
        }

        private void abrirjanela_confirm(object obj)
        {
            Application.Run(new Confirm_Logout());
        }

        private void abrirjanela_pass(object obj)
        {
            Application.Run(new Change_Password());
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            t1 = new Thread(abrirjanela_pass);
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
        }
    }
}
