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
    public partial class Register : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");

        Thread t1;
        

        public Register()
        {
            InitializeComponent();
        }

        private void btregister_Click(object sender, EventArgs e)
        {
            if (tb_name.Text == "" || tb_user.Text == "" || tb_email.Text == "" || tb_pass.Text == "")
            {
                MessageBox.Show("Preencha todos os campos!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (tb_pass.Text.Length < 8)
                {
                    MessageBox.Show("Introduza uma password com pelo menos 8 caracteres!","Aviso", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        con.Open();
                        string comando = "INSERT INTO Account (username, password, name, email) VALUES (@user,HASHBYTES('SHA2_512','" + tb_pass.Text + "'), @name, '" + tb_email.Text + "')";

                        SqlParameter param = new SqlParameter();
                        SqlParameter param1 = new SqlParameter();
                        SqlParameter param2 = new SqlParameter();


                        param.ParameterName = "@user";
                        // param1.ParameterName = "@pass";
                        param2.ParameterName = "@name";


                        param.Value = tb_user.Text;
                        //param1.Value = tb_pass.Text;
                        param2.Value = tb_name.Text;


                        SqlCommand cmd = con.CreateCommand();


                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add(param);
                        //cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param2);


                        cmd.CommandText = comando;


                        try
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Utilizador registado com sucesso!", "Ação finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Home.username = tb_user.Text;

                            this.Close();
                            t1 = new Thread(abrirjanela_home);
                            t1.SetApartmentState(ApartmentState.STA);
                            t1.Start();
                        }
                        catch (SqlException s)
                        {
                            MessageBox.Show("Erro no Comando SQL: " + s.Message.ToString());
                        }
                        con.Close();

                    }
                    catch (SqlException s)
                    {
                        MessageBox.Show("Erro no Comando SQL: " + s.Message.ToString());
                    }
                }
            }
        }

        private void Register_Load(object sender, EventArgs e)
        {
            
        }

        private void abrirjanela_home()
        {
            Application.Run(new Home());
        }

        private void abrirjanela_main()
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

        private bool dragging;
        private Point pointClicked;

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
