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
using Guna.UI.WinForms;
using System.Net;

namespace PAP
{
    public partial class New_Trade : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");
        SqlConnection con1 = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");

        string first_pair;
        string second_pair;
        int cod_pair;
        string pair;

        string stop;

        int cod_type;
        double entry;
        double entry2;
        double close;
        double lot;
        double profit;
        double pips_made;

        int y;


        public New_Trade()
        {
            InitializeComponent();
        }

        private void label_date_Click(object sender, EventArgs e)
        {

        }

        private void New_Trade_Load(object sender, EventArgs e)
        {
            gunaDateTimePicker1.Value = DateTime.Now;
            label14.Text = DateTime.Now.ToShortDateString();

            try
            {
                SqlDataReader reader = null;
                try
                {
                    con.Open();
                    string comando = "Select DISTINCT first_pair from Pair";

                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = comando;

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        gunaComboBox1.Items.Add(reader.GetString(0));
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            gunaComboBox2.Items.Clear();
            gunaComboBox2.Enabled = true;

            first_pair = gunaComboBox1.Text;

            try
            {
                SqlDataReader reader = null;
                try
                {
                    con.Open();
                    string comando = "Select second_pair, id_pair from Pair WHERE first_pair like @pair";

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@pair";
                    param.Value = first_pair;


                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(param);
                    cmd.CommandText = comando;

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        gunaComboBox2.Items.Add(reader.GetString(0));
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            second_pair = gunaComboBox2.Text;

            try
            {
                SqlDataReader reader = null;
                try
                {
                    con.Open();
                    string comando = "Select id_pair from Pair WHERE first_pair like @pair AND second_pair like @pair1";

                    SqlParameter param = new SqlParameter();
                    SqlParameter param1 = new SqlParameter();
                    param.ParameterName = "@pair";
                    param1.ParameterName = "@pair1";
                    param.Value = first_pair;
                    param1.Value = second_pair;


                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(param);
                    cmd.Parameters.Add(param1);
                    cmd.CommandText = comando;

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cod_pair = reader.GetInt32(0);
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
            if (gunaComboBox2.Text!="")
            {
                cod_type = 1;

                pair = gunaComboBox1.Text + gunaComboBox2.Text;

                label1.Text = pair;
                label13.Text = pair;
                label18.Text = "BUY";

                #region Show
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                gunaDateTimePicker1.Visible = true;
                gunaTextBox1.Visible = true;
                gunaTextBox2.Visible = true;
                gunaTextBox3.Visible = true;
                gunaPanel1.Visible = true;
                gunaButton4.Visible = true;



                label6.Visible = false;
                label7.Visible = false;
                gunaButton1.Visible = false;
                gunaButton2.Visible = false;
                gunaButton3.Visible = false;
                gunaTextBox4.Visible = false;
                gunaComboBox1.Visible = false;
                gunaComboBox2.Visible = false;
                #endregion

            }
            else
            {
                MessageBox.Show("Selecione primeiro o par que pretende!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            if (gunaComboBox2.Text != "")
            {
                cod_type = 3;

                pair = gunaComboBox1.Text + gunaComboBox2.Text;

                label1.Text = pair;
                label13.Text = pair;
                label18.Text = "Breakeven";

                #region Show
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label5.Visible = true;
                gunaDateTimePicker1.Visible = true;
                gunaTextBox3.Visible = true;
                gunaTextBox4.Visible = true;
                gunaPanel1.Visible = true;
                gunaButton4.Visible = true;



                label6.Visible = false;
                label7.Visible = false;
                gunaButton1.Visible = false;
                gunaButton2.Visible = false;
                gunaButton3.Visible = false;
                gunaComboBox1.Visible = false;
                gunaComboBox2.Visible = false;
                #endregion
            }
            else
            {
                MessageBox.Show("Selecione primeiro o par que pretende!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            if (gunaComboBox2.Text != "")
            {
                cod_type = 2;

                pair = gunaComboBox1.Text + gunaComboBox2.Text;

                label1.Text = pair;
                label13.Text = pair;
                label18.Text = "SELL";

                #region Show
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                gunaDateTimePicker1.Visible = true;
                gunaTextBox1.Visible = true;
                gunaTextBox2.Visible = true;
                gunaTextBox3.Visible = true;
                gunaPanel1.Visible = true;
                gunaButton4.Visible = true;



                label6.Visible = false;
                label7.Visible = false;
                gunaButton1.Visible = false;
                gunaButton2.Visible = false;
                gunaButton3.Visible = false;
                gunaTextBox4.Visible = false;
                gunaComboBox1.Visible = false;
                gunaComboBox2.Visible = false;
                #endregion
            }
            else
            {
                MessageBox.Show("Selecione primeiro o par que pretende!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gunaTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as GunaTextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void gunaTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (gunaTextBox2.Text.Length != 0)
            {
                if (gunaTextBox2.Text.Substring(0, 1) == ".")
                {
                    gunaTextBox2.Text = "0.";
                }

                gunaTextBox2.Focus();
                gunaTextBox2.SelectionStart = gunaTextBox2.Text.Length;

                stop = gunaTextBox2.Text;
                stop = stop.Replace(".", ",");

                close = Convert.ToDouble(stop);

                label16.Text = stop.Replace(",", ".");

            }
            else
            {
                label16.Text = "0";
            }
        }

        private void gunaTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (gunaTextBox1.Text.Length != 0)
            {
                if (gunaTextBox1.Text.Substring(0, 1) == ".")
                {
                    gunaTextBox1.Text = "0.";
                }

                gunaTextBox1.Focus();
                gunaTextBox1.SelectionStart = gunaTextBox1.Text.Length;

                stop = gunaTextBox1.Text;
                stop = stop.Replace(".", ",");

                entry = Convert.ToDouble(stop);

                label15.Text = stop.Replace(",", ".");
            }
            else
            {
                label15.Text = "0";
            }
        }

        private void gunaTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as GunaTextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void gunaTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as GunaTextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

           
        }

        private void gunaTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (gunaTextBox3.Text.Length != 0)
            {
                if (gunaTextBox3.Text.Substring(0, 1) == ".")
                {
                    gunaTextBox3.Text = "0.";
                }

                gunaTextBox3.Focus();
                gunaTextBox3.SelectionStart = gunaTextBox3.Text.Length;

                stop = gunaTextBox3.Text;
                stop = stop.Replace(".", ",");

                lot = Convert.ToDouble(stop);

                label17.Text = stop.Replace(",", ".");
            }
            else
            {
                label17.Text = "0";
            }

            if (cod_type==3)
            {
                profit = 0;
            }
            else
            {
                if (first_pair == "EUR")
                {
                    if (second_pair != "JPY")
                    {
                        if (cod_type==1)
                        {
                            profit = 0.0001 / entry * 100000 * lot * (close - entry) * 10000;
                            profit = Math.Round(profit, 2);
                            pips_made = Math.Round(((close - entry) * 10000), MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            profit = 0.0001 / entry * 100000 * lot * (entry - close) * 10000;
                            profit = Math.Round(profit, 2);
                            pips_made = Math.Round(((entry - close) * 10000), MidpointRounding.AwayFromZero);
                        }

                        if (profit==0)
                        {
                            label21.ForeColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            if (profit>0)
                            {
                                label21.ForeColor = System.Drawing.Color.Lime;
                            }
                            else
                            {
                                label21.ForeColor = System.Drawing.Color.Red;
                            }
                        }

                        label21.Text = profit.ToString() + "€";
                    }
                    else
                    {
                        if (cod_type == 1)
                        {
                            profit = 0.01 / entry * 100000 * lot * (close - entry) * 100;
                            profit = Math.Round(profit, 2);
                            pips_made = Math.Round(((close - entry) * 100), MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            profit = 0.01 / entry * 100000 * lot * (entry-close) * 100;
                            profit = Math.Round(profit, 2);
                            pips_made = Math.Round(((entry - close) * 100), MidpointRounding.AwayFromZero);
                        }

                        label21.Text = profit.ToString() + "€";

                        if (profit == 0)
                        {
                            label21.ForeColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            if (profit > 0)
                            {
                                label21.ForeColor = System.Drawing.Color.Lime;
                            }
                            else
                            {
                                label21.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                }
                else
                {
                    //nenhuma das duas é euro

                    WebClient wc = new WebClient();

                    string[] words = wc.DownloadString("http://data.fixer.io/api/latest?access_key=adbe5a82d59a9ac71b2b7bf187ead4c7&symbols=AUD,CAD,CHF,GBP,JPY,NZD,USD&format=1").Split('{');
                    string[] values = words[2].Split(':', ',', '}');

                    switch (second_pair)
                    {
                        case "AUD":
                            y = 1;
                            break;

                        case "CAD":
                            y = 3;
                            break;

                        case "CHF":
                            y = 5;
                            break;

                        case "GBP":
                            y = 7;
                            break;

                        case "JPY":
                            y = 9;
                            break;

                        case "NZD":
                            y = 11;
                            break;

                        case "USD":
                            y = 13;
                            break;

                        default:
                            y = -1;
                            break;
                    }

                    if (second_pair != "JPY")
                    {
                        stop = values[y].Replace('.', ',');
                        entry2 = Convert.ToDouble(stop);

                        if (cod_type == 1)
                        {
                            profit = 0.0001 / entry2 * 100000 * lot * (close - entry) * 10000;
                            profit = Math.Round(profit, 2);
                            pips_made = Math.Round(((close - entry) * 10000), MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            profit = 0.0001 / entry2 * 100000 * lot * (entry - close) * 10000;
                            profit = Math.Round(profit, 2);
                            pips_made = Math.Round(((entry - close) * 10000), MidpointRounding.AwayFromZero);
                        }

                        
                        profit = Math.Round(profit, 2);


                        if (profit == 0)
                        {
                            label21.ForeColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            if (profit > 0)
                            {
                                label21.ForeColor = System.Drawing.Color.Lime;
                            }
                            else
                            {
                                label21.ForeColor = System.Drawing.Color.Red;
                            }
                        }

                        label21.Text = profit.ToString() + "€";
                    }
                    else
                    {
                        stop = values[y].Replace('.', ',');
                        entry2 = Convert.ToDouble(stop);

                        if (cod_type == 1)
                        {
                            profit = 0.01 / entry2 * 100000 * lot * (close - entry) * 100;
                            profit = Math.Round(profit, 2);
                            pips_made = Math.Round(((close - entry) * 100), MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            profit = 0.01 / entry2 * 100000 * lot * (entry - close) * 100;
                            profit = Math.Round(profit, 2);
                            pips_made = Math.Round(((entry - close) * 100), MidpointRounding.AwayFromZero);
                        }

                        if (profit == 0)
                        {
                            label21.ForeColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            if (profit > 0)
                            {
                                label21.ForeColor = System.Drawing.Color.Lime;
                            }
                            else
                            {
                                label21.ForeColor = System.Drawing.Color.Red;
                            }
                        }

                        label21.Text = profit.ToString() + "€";
                    }
                }
            }

            
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void gunaTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (gunaTextBox4.Text.Length != 0)
            {
                if (gunaTextBox4.Text.Substring(0, 1) == ".")
                {
                    gunaTextBox4.Text = "0.";
                }

                gunaTextBox4.Focus();
                gunaTextBox4.SelectionStart = gunaTextBox4.Text.Length;

                stop = gunaTextBox4.Text;
                stop = stop.Replace(".", ",");

                entry = Convert.ToDouble(stop);

                label15.Text = stop.Replace(",", ".");
                label16.Text = stop.Replace(",", ".");
            }
            else
            {
                label15.Text = "0";
                label16.Text = "0";
            }
        }

        private void gunaTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as GunaTextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void gunaDateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            label14.Text = gunaDateTimePicker1.Text;
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void gunaButton4_Click(object sender, EventArgs e)
        {
            #region INSERT TRADE

            SqlDataReader reader = null;
            try
            {
                con.Open();
                double balance_antigo = 0;
                int pips_antigo = 0;
                int n_trades = 0;

                int uid = Home.uid;

                string comando = "select balance, total_pips, n_trades from Account where uid = @uid";

                SqlCommand cmd = con.CreateCommand();
                cmd.Parameters.AddWithValue("@uid", uid);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = comando;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    balance_antigo = reader.GetDouble(0);
                    pips_antigo = reader.GetInt32(1);
                    n_trades = reader.GetInt32(2);
                }
                con.Close();

                int uid1 = Home.uid;
                double lot_size = lot;
                int type = cod_type;
                int pair = cod_pair;
                double entry_price = entry;
                double close_price = close;
                double profit_loss = profit;
                double pips = pips_made;
                int ntrades = n_trades + 1;

                DateTime dat = gunaDateTimePicker1.Value;
                string datData = dat.ToString("yyyy-MM-dd");

                double balance_novo = balance_antigo + profit;
                Math.Round(balance_novo, 2);

                string balance = balance_novo.ToString().Replace(',', '.'); //para o caso de ter colocado virgula em vez de ponto pois para inserir tem que estar com '.'

                string comando1 = "INSERT INTO Trade (uid, date_trade, lot_size, id_type, id_pair, entry_price, close_price, profit_loss, pips_made, balance) VALUES (@uid, @date, @lot, @type, @pair, @entry, @close, @profit, @pips, @balance)";
                SqlCommand cmd1 = new SqlCommand(comando1, con);
                cmd1.Parameters.AddWithValue("@uid", uid1);
                cmd1.Parameters.AddWithValue("@date", datData);
                cmd1.Parameters.AddWithValue("@lot", lot_size);
                cmd1.Parameters.AddWithValue("@type", type);
                cmd1.Parameters.AddWithValue("@pair", pair);
                cmd1.Parameters.AddWithValue("@entry", entry_price);
                cmd1.Parameters.AddWithValue("@close", close_price);
                cmd1.Parameters.AddWithValue("@profit", profit_loss);
                cmd1.Parameters.AddWithValue("@pips", pips);
                cmd1.Parameters.AddWithValue("@balance", balance);

                cmd1.CommandType = CommandType.Text;

                con.Open();

                try
                {
                    cmd1.ExecuteNonQuery();
                }
                catch (SqlException s)
                {
                    MessageBox.Show("Erro no comando SQL: " + s.Message.ToString());
                }
                con.Close();

                comando = "UPDATE Account SET balance = @bal, total_pips = @pips, n_trades = @trades WHERE uid = @uid";
                con.Open();
                SqlCommand cmd2 = new SqlCommand(comando, con);

                double pips_novo = pips_antigo + pips;
                

                cmd2.Parameters.AddWithValue("@uid", uid);
                cmd2.Parameters.AddWithValue("@bal", balance);
                cmd2.Parameters.AddWithValue("@pips", pips_novo);
                cmd2.Parameters.AddWithValue("@trades", ntrades);
                cmd2.CommandType = CommandType.Text;

                try
                {
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("Operação registada com sucesso!", "Ação finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (con != null)
                { con.Close(); }
            }

            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            gunaDateTimePicker1.Visible = false;
            gunaTextBox1.Visible = false;
            gunaTextBox2.Visible = false;
            gunaTextBox3.Visible = false;
            gunaPanel1.Visible = false;
            gunaButton4.Visible = false;
            gunaTextBox4.Visible = false;



            label6.Visible = true;
            label7.Visible = true;
            gunaButton1.Visible = true;
            gunaButton2.Visible = true;
            gunaButton3.Visible = true;
            gunaComboBox1.Visible = true;
            gunaComboBox2.Visible = true;

            
            gunaComboBox1.SelectedIndex = -1;
            gunaComboBox2.SelectedIndex = -1;

            gunaTextBox1.Text = "";
            gunaTextBox2.Text = "";
            gunaTextBox3.Text = "";
            gunaTextBox4.Text = "";

            gunaComboBox2.Enabled = false;

            label21.Text = "0.00€";
            label21.ForeColor = System.Drawing.Color.White;

            #endregion
        }
    }
}
