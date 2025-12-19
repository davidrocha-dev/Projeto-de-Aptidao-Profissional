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
    public partial class Change_Password : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");

        public Change_Password()
        {
            InitializeComponent();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            //confirmar pass atual com base de dados



            //confirmar pass nova uma com a outra

            if (tb_passnova.Text == tb_confirmpass.Text)
            {
                #region ALTERAR

                try
                {
                    con.Open();
                    string comando = "UPDATE Account SET password = HASHBYTES('SHA2_512', '" + tb_passnova.Text + "') WHERE uid = @uid AND password = HASHBYTES('SHA2_512', '" + tb_passatual.Text + "')";

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@uid";
                    param.Value = Home.uid;

                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(param);
                    cmd.CommandText = comando;

                    try
                    {
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Palavra-passe alterada com sucesso!", "Informação",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    catch (SqlException s)
                    {
                        MessageBox.Show("Erro no Comando SQL: " + s.Message.ToString());
                    }

                }
                catch (SqlException s)
                {
                    MessageBox.Show("Erro no Comando SQL: " + s.Message.ToString());
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }

                #endregion
            }
            else
            {
                MessageBox.Show("As Palavras-Passe não coincidem!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void gunaPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
