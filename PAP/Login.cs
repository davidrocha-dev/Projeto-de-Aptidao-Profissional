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

namespace PAP
{
    public partial class Login : Form
    {
        private bool dragging;
        private Point pointClicked;

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");

        Thread t1;

        public Login()
        {
            InitializeComponent();
        }

        private void abrirjanela_home()
        {
            Application.Run(new Home());
        }

        private void btlogin_Click(object sender, EventArgs e)
        {
            string user = tb_user.Text;
            string pass = tb_pass.Text;
            bool login = false;

            try
            {
                SqlDataReader reader = null;
                try
                {
                    con.Open();
                    //  string comando = "select password, username from Account WHERE username = @user AND password = HASHBYTES('SHA2_512','@pass')";
                    string comando = "Select username, [password], CASE[password] WHEN HASHBYTES('SHA2_512', '" + tb_pass.Text + "') THEN CASE[username] WHEN '" + tb_user.Text + "' THEN '1' ELSE '0' END ELSE '0' END As Status from Account";


                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = comando;

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader[2].ToString() == "1")
                        {
                            login = true;
                        }
                    }


                    if (login)
                    {
                        t1 = new Thread(abrirjanela_home);
                        t1.SetApartmentState(ApartmentState.STA);
                        t1.Start();
                        Home.username = tb_user.Text;
                        this.Close();
                        login = false;
                    }
                    else
                    {
                        MessageBox.Show("Username ou Password Incorretos!");
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

        private void Login_Load(object sender, EventArgs e)
        {

        }


        //Main
        private void abrirjanela_main(object obj)
        {
            Application.Run(new Main());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            t1 = new Thread(abrirjanela_main);
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
        }

        private void gunaPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {

                Point pointMoveTo;

                // Find the current mouse position in screen coordinates.
                pointMoveTo = this.PointToScreen(new Point(e.X, e.Y));

                // Compensate for the position the control was clicked.
                pointMoveTo.Offset(-pointClicked.X, -pointClicked.Y);

                // Move the form.
                this.Location = pointMoveTo;
            }
        }

        private void gunaPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                dragging = true;
                pointClicked = new Point(e.X, e.Y);
            }
            else
            {

                dragging = false;
            }
        }

        private void gunaPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void gunaPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gunaPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
