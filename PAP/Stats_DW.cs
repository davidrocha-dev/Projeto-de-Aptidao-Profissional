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
    public partial class Stats_DW : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");

        public Stats_DW()
        {
            InitializeComponent();
        }

        private void Stats_DW_Load(object sender, EventArgs e)
        {
            
            SqlDataReader reader = null;
            try
            {
                con.Open();
                string command = "SELECT T.id, T.date_trans, TT.desc_type, T.tvalue, T.balance FROM Transactions T, Type_Trans TT WHERE T.id_type = TT.id_type AND T.uid = @uid";

                SqlParameter param = new SqlParameter();

                param.ParameterName = "@uid";

                param.Value = Home.uid;

                SqlCommand cmd = con.CreateCommand();

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(param);
                cmd.CommandText = command;

                reader = cmd.ExecuteReader();

                string[] linhaDados = new string[5];
                while (reader.Read())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (reader.GetFieldType(i).ToString() == "System.Int32")
                        {
                            linhaDados[i] = reader.GetInt32(i).ToString();
                        }
                        if (reader.GetFieldType(i).ToString() == "System.String")
                        {
                            linhaDados[i] = reader.GetString(i).ToString();
                        }
                        if (reader.GetFieldType(i).ToString() == "System.DateTime")
                        {
                            linhaDados[i] = reader.GetDateTime(i).ToString();
                        }
                        if (reader.GetFieldType(i).ToString() == "System.Double")
                        {
                            linhaDados[i] = reader.GetDouble(i).ToString();
                        }
                    }
                    gunaDataGridView1.Rows.Add(linhaDados);
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

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
