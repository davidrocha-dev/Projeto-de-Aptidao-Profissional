using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;

namespace PAP
{
    public partial class Change_Image : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");


        public Change_Image()
        {
            InitializeComponent();
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    gunaTextBox1.Text = ofd.FileName;
                    gunaCirclePictureBox1.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void gunaControlBox2_Click(object sender, EventArgs e)
        {

        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            if (gunaTextBox1.Text != "")
            {
                Image img = gunaCirclePictureBox1.Image;
                byte[] arr;
                ImageConverter converter = new ImageConverter();
                arr = (byte[])converter.ConvertTo(img, typeof(byte[]));

                var image = new ImageConverter().ConvertTo(gunaCirclePictureBox1.Image, typeof(Byte[]));
                con.Open();

                string comando = "UPDATE Account set profile_photo = @image WHERE uid = @uid";
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@uid";
                param.Value = Home.uid;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(param);

                cmd.CommandText = comando;
                cmd.Parameters.AddWithValue("@image", arr);
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Imagem alterada com sucesso!", "Ação finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException s)
                {
                    MessageBox.Show(s.Message.ToString());
                }
                con.Close();

                this.Close();
            }
            else
            {
                MessageBox.Show("Carregue uma imagem primeiro!","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private bool dragging;
        private Point pointClicked;


        private void gunaPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gunaPanel2_MouseDown(object sender, MouseEventArgs e)
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

        private void gunaPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Change_Image_Load(object sender, EventArgs e)
        {
            try
            {
                SqlDataReader reader = null;
                try
                {
                    con.Open();
                    string command = "SELECT profile_photo FROM Account WHERE uid = @uid";

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
                        if (reader.GetSqlBytes(0).Buffer != null)
                        {
                            MemoryStream str = new MemoryStream(reader.GetSqlBytes(0).Buffer);
                            gunaCirclePictureBox1.Image = Image.FromStream(str);
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
    }
}
