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
    public partial class Stats : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");
        SqlConnection con1 = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");


        string valor;
        string pips;

        public Stats()
        {
            InitializeComponent();
        }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Stats_Load(object sender, EventArgs e)
        {

            SqlDataReader reader = null;
            try
            {
                con.Open();
                string command = "SELECT T.trade_id, T.date_trade, T.lot_size, TT.desc_type, P.first_pair, P.second_pair, T.entry_price, T.close_price, T.pips_made, T.profit_loss, T.balance FROM Trade T, Type_Trade TT, Pair P WHERE T.id_type = TT.id_type AND T.id_pair = P.id_pair AND T.uid = @uid";

                SqlParameter param = new SqlParameter();

                param.ParameterName = "@uid";

                param.Value = Home.uid;

                SqlCommand cmd = con.CreateCommand();

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(param);
                cmd.CommandText = command;

                reader = cmd.ExecuteReader();

                string[] linhaDados = new string[11];

                while (reader.Read())
                {
                    linhaDados[0] = reader.GetInt32(0).ToString();
                    linhaDados[1] = reader.GetDateTime(1).ToString("dd/MM/yyyy");
                    linhaDados[2] = reader.GetDouble(2).ToString();
                    linhaDados[3] = reader.GetString(3).ToString();
                    linhaDados[4] = reader.GetString(4).ToString() + reader.GetString(5).ToString();
                    linhaDados[5] = reader.GetDouble(6).ToString();
                    linhaDados[6] = reader.GetDouble(7).ToString();
                    linhaDados[7] = reader.GetInt32(8).ToString();
                    linhaDados[8] = reader.GetDouble(9).ToString();
                    linhaDados[9] = reader.GetDouble(10).ToString();

                    gunaDataGridView1.Rows.Add(linhaDados);

                    Array.Clear(linhaDados, 0, linhaDados.Length);
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

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            if (gunaDataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Selecione uma Trade primeiro!","Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    int selectedrowindex = gunaDataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = gunaDataGridView1.Rows[selectedrowindex];

                    con.Open();
                    string comando = "DELETE FROM Trade WHERE trade_id = @id";

                    valor = Convert.ToString(selectedRow.Cells["ProfitLoss"].Value);
                    pips = Convert.ToString(selectedRow.Cells["pips_trade"].Value);

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@id";
                    param.Value = Convert.ToString(selectedRow.Cells["Trade_id"].Value);

                    SqlCommand cmd = con.CreateCommand();
                    cmd.Parameters.Add(param);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = comando;

                    try
                    {

                        DialogResult x = MessageBox.Show("Deseja apagar a Operação?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (x == DialogResult.Yes)
                        {
                            cmd.ExecuteNonQuery();

                            #region INSERT Transactions

                            SqlDataReader reader1 = null;

                            try
                            {
                                con1.Open();
                                double balance_antigo = 0;

                                int uid = Home.uid;
                                int type_trade = 3;

                                comando = "select balance from Account where uid = @uid";

                                cmd = con1.CreateCommand();
                                cmd.Parameters.AddWithValue("@uid", uid);
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = comando;

                                reader1 = cmd.ExecuteReader();

                                while (reader1.Read())
                                {
                                    balance_antigo = reader1.GetDouble(0);
                                }
                                con1.Close();

                                string balance = valor.Replace('.', ','); //para converter em double tem que estar com ,

                                double balance_novo = Convert.ToDouble(balance) * -1;

                                balance_antigo = balance_antigo + balance_novo;


                                comando = "INSERT INTO Transactions(uid, id_type, tvalue, balance) VALUES(@uid, @type, @value, @bal)";
                                con1.Open();
                                SqlCommand cmd1 = new SqlCommand(comando, con1);

                                cmd1.Parameters.AddWithValue("@uid", uid);
                                cmd1.Parameters.AddWithValue("@type", type_trade);
                                cmd1.Parameters.AddWithValue("@value", balance_novo);
                                cmd1.Parameters.AddWithValue("@bal", balance_antigo);
                                cmd1.CommandType = CommandType.Text;

                                try
                                {
                                    cmd1.ExecuteNonQuery();
                                }
                                catch (SqlException s)
                                {
                                    MessageBox.Show("Erro no comando SQL: " + s.Message.ToString());
                                }
                            }
                            catch (SqlException s)
                            {
                                MessageBox.Show("Erro no comando SQL: " + s.Message.ToString());
                            }
                            finally
                            {
                                // Fecha o datareader
                                if (reader1 != null)
                                { reader1.Close(); }
                                // Fecha a conexão
                                if (con1 != null)
                                { con1.Close(); }
                            }

                            #endregion

                            #region UPDATE Account

                            SqlDataReader reader = null;
                            try
                            {
                                con1.Open();
                                double balance_antigo = 0;
                                double pips_conta = 0;
                                int n_trades = 0;

                                int uid = Home.uid;

                                comando = "select balance, total_pips, n_trades from Account where uid = @uid";

                                cmd = con1.CreateCommand();
                                cmd.Parameters.AddWithValue("@uid", uid);
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = comando;

                                reader = cmd.ExecuteReader();

                                while (reader.Read())
                                {
                                    balance_antigo = reader.GetDouble(0);
                                    pips_conta = reader.GetInt32(1);
                                    n_trades = reader.GetInt32(2);
                                }
                                con1.Close();

                                string balance = valor.Replace('.', ','); //para converter em double tem que estar com ,
                                double balance_novo = balance_antigo + (Convert.ToDouble(balance) * (-1));

                                double total_pips = pips_conta + Convert.ToDouble(pips) * (-1);
                                n_trades = n_trades - 1;

                                comando = "update Account set balance = @bal, total_pips = @pips, n_trades = @trades where uid = @uid";
                                con1.Open();
                                SqlCommand cmd1 = new SqlCommand(comando, con1);

                                cmd1.Parameters.AddWithValue("@bal", balance_novo);
                                cmd1.Parameters.AddWithValue("@uid", uid);
                                cmd1.Parameters.AddWithValue("@pips", total_pips);
                                cmd1.Parameters.AddWithValue("@trades", n_trades);
                                cmd1.CommandType = CommandType.Text;

                                try
                                {
                                    cmd1.ExecuteNonQuery();
                                }
                                catch (SqlException s)
                                {
                                    MessageBox.Show("Erro no comando SQL: " + s.Message.ToString());
                                }
                            }
                            catch (SqlException s)
                            {
                                MessageBox.Show("Erro no comando SQL: " + s.Message.ToString());
                            }
                            finally
                            {
                                // Fecha o datareader
                                if (reader != null)
                                { reader.Close(); }
                                // Fecha a conexão
                                if (con1 != null)
                                { con1.Close(); }
                            }

                            #endregion

                            gunaDataGridView1.Rows.Clear();

                            #region Refresh

                            reader = null;
                            try
                            {
                                con1.Open();
                                string command = "SELECT T.trade_id, T.date_trade, T.lot_size, TT.desc_type, P.first_pair, P.second_pair, T.entry_price, T.close_price, T.pips_made, T.profit_loss, T.balance FROM Trade T, Type_Trade TT, Pair P WHERE T.id_type = TT.id_type AND T.id_pair = P.id_pair AND T.uid = @uid";

                                SqlParameter param1 = new SqlParameter();

                                param1.ParameterName = "@uid";

                                param1.Value = Home.uid;

                                cmd = con1.CreateCommand();

                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add(param1);
                                cmd.CommandText = command;

                                reader = cmd.ExecuteReader();

                                string[] linhaDados = new string[11];

                                while (reader.Read())
                                {
                                    linhaDados[0] = reader.GetInt32(0).ToString();
                                    linhaDados[1] = reader.GetDateTime(1).ToString("dd/MM/yyyy");
                                    linhaDados[2] = reader.GetDouble(2).ToString();
                                    linhaDados[3] = reader.GetString(3).ToString();
                                    linhaDados[4] = reader.GetString(4).ToString() + reader.GetString(5).ToString();
                                    linhaDados[5] = reader.GetDouble(6).ToString();
                                    linhaDados[6] = reader.GetDouble(7).ToString();
                                    linhaDados[7] = reader.GetInt32(8).ToString();
                                    linhaDados[8] = reader.GetDouble(9).ToString();
                                    linhaDados[9] = reader.GetDouble(10).ToString();

                                    gunaDataGridView1.Rows.Add(linhaDados);

                                    Array.Clear(linhaDados, 0, linhaDados.Length);
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
                                if (con1 != null)
                                {
                                    con1.Close();
                                }
                            }

                            #endregion

                            MessageBox.Show("Operação eliminada com sucesso!", "Ação finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
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
            }   
        }
    }
}
