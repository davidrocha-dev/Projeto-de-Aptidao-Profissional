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

namespace PAP
{
    public partial class Home : Form
    {
        

        private bool dragging;
        private Point pointClicked;

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");


        public static string username;
        public static int uid;

        public Home()
        {
            InitializeComponent();
        }

        #region Main Design

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            home_panel.Controls.Add(childForm);
            home_panel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        #endregion


        private void Home_Load(object sender, EventArgs e)
        {
            
            openChildForm(new HomeForm());

            try
            {
                SqlDataReader reader = null;
                try
                {
                    con.Open();
                    string comando = "select uid from Account WHERE username = @user";

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@user";
                    param.Value = username;

                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(param);
                    cmd.CommandText = comando;

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        uid=reader.GetInt32(0);
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

        private void bt_home_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void bt_home_MouseLeave(object sender, EventArgs e)
        {
            
        }

        private void bt_home_Click(object sender, EventArgs e)
        {
            openChildForm(new HomeForm());
        }

        private void bt_add_Click(object sender, EventArgs e)
        {
            openChildForm(new New_Trade());
        }

        private void bt_dep_wit_Click(object sender, EventArgs e)
        {
            openChildForm(new Deposit_Withdraw());
        }

        private void bt_stats_Click(object sender, EventArgs e)
        {
            openChildForm(new Stats());
        }



        private void bt_user_Click(object sender, EventArgs e)
        {
            openChildForm(new Account());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void home_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gunaPanel1_Move(object sender, EventArgs e)
        {

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

        private void gunaPictureBox1_Click(object sender, EventArgs e)
        {
            openChildForm(new HomeForm());
            gunaAdvenceButton1.Checked = true;
        }

        private void home_panel_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
