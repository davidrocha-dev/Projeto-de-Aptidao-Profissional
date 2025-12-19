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
using System.IO;

namespace PAP
{
    public partial class Deposit_Withdraw : Form
    {
        #region Main Design

        private Form activeForm = null;

        #endregion

        int op;


        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=S:\01 - David Rocha - Trading Journal\Projeto\Projetos\db_journal.mdf;Integrated Security=True;Connect Timeout=30");


        public Deposit_Withdraw()
        {
            InitializeComponent();
        }

        private void Deposit_Withdraw_Load(object sender, EventArgs e)
        {
            panel_dw.Show();

            try
            {
                SqlDataReader reader = null;
                try
                {
                    double balance_antigo = 0;

                    con.Open();
                    string command = "SELECT name, balance, profile_photo, symbol_currency FROM Account, Currency WHERE Account.id_currency = Currency.id_currency AND uid = @uid";

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
                        label4.Text = (reader.GetString(0));

                        balance_antigo = (reader.GetDouble(1));

                        label3.Text = string.Format("{0:#,0.00}", balance_antigo) + reader.GetString(3);

                        if (reader.GetSqlBytes(2).Buffer != null)
                        {
                            MemoryStream str = new MemoryStream(reader.GetSqlBytes(2).Buffer);
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

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            op = 1; //Check se é depositar ou levantar

            #region Tools Stuff

            gunaButton1.Visible = false;
            gunaButton2.Visible = false;
            gunaTextBox1.Visible = true;
            label1.Text = "Introduza o montante que pretende depositar";
            gunaCirclePictureBox1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            gunaButton3.Visible = true;
            gunaButton4.Visible = true;
            gunaButton5.Visible = true;
            gunaButton6.Visible = true;
            gunaButton7.Visible = true;
            gunaButton8.Visible = true;
            gunaButton9.Visible = true;
            gunaButton10.Visible = true;
            gunaButton11.Visible = false;

            #endregion
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            op = 2; //Check se é depositar ou levantar

            #region Tools Stuff

            gunaButton1.Visible = false;
            gunaButton2.Visible = false;
            gunaTextBox1.Visible = true;
            label1.Text = "Introduza o montante que pretende levantar";
            gunaCirclePictureBox1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            gunaButton3.Visible = true;
            gunaButton4.Visible = true;
            gunaButton5.Visible = true;
            gunaButton6.Visible = true;
            gunaButton7.Visible = true;
            gunaButton8.Visible = true;
            gunaButton9.Visible = true;
            gunaButton10.Visible = true;
            gunaButton11.Visible = false;

            #endregion
        }

        private void gunaButton5_Click(object sender, EventArgs e)
        {
            gunaTextBox1.ReadOnly = true;
            gunaTextBox1.Text = "200";
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            gunaTextBox1.ReadOnly = true;
            
            gunaTextBox1.Text="50";
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            gunaTextBox1.Clear();
            gunaTextBox1.ReadOnly = false;
        }

        private void gunaButton4_Click(object sender, EventArgs e)
        {
            gunaTextBox1.ReadOnly = true;
            gunaTextBox1.Text = "100";
        }

        private void gunaButton6_Click(object sender, EventArgs e)
        {
            gunaTextBox1.ReadOnly = true;
            gunaTextBox1.Text = "500";
        }

        private void gunaButton7_Click(object sender, EventArgs e)
        {
            gunaTextBox1.ReadOnly = true;
            gunaTextBox1.Text = "1000";
        }

        private void gunaButton10_Click(object sender, EventArgs e)
        {
            #region Tools Stuff

            gunaButton1.Visible = true;
            gunaButton2.Visible = true;
            gunaTextBox1.Visible = false;
            label1.Text = "Escolha a opção que prentende:";
            gunaCirclePictureBox1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            gunaButton3.Visible = false;
            gunaButton4.Visible = false;
            gunaButton5.Visible = false;
            gunaButton6.Visible = false;
            gunaButton7.Visible = false;
            gunaButton8.Visible = false;
            gunaButton9.Visible = false;
            gunaButton10.Visible = false;
            gunaButton11.Visible = true;

            #endregion
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

        private void gunaButton9_Click(object sender, EventArgs e)
        {
            if (op == 1) //Depósito
            {
                #region Tools Stuff

                gunaButton9.Visible = false;
                gunaButton10.Visible = false;

                #endregion

                //Update ao balance em Account

                #region UPDATE Account

                SqlDataReader reader = null;
                try
                {
                    con.Open();
                    double balance_antigo = 0;

                    int uid = Home.uid;

                    string comando = "select balance from Account where uid = @uid";

                    SqlCommand cmd = con.CreateCommand();
                    cmd.Parameters.AddWithValue("@uid", uid);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = comando;

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        balance_antigo = reader.GetDouble(0);
                    }
                    con.Close();

                    string balance = gunaTextBox1.Text.Replace('.', ','); //para converter em double tem que estar com ,
                    double balance_novo = balance_antigo + Convert.ToDouble(balance);

                    comando = "update Account set balance = @bal where uid = @uid";
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand(comando, con);

                    cmd1.Parameters.AddWithValue("@bal", balance_novo);
                    cmd1.Parameters.AddWithValue("@uid", uid);
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
                    if (con != null)
                    { con.Close(); }
                }

                #endregion

                //adicionar as transações

                #region INSERT Transactions

                reader = null;

                try
                {
                    con.Open();
                    double balance_antigo = 0;

                    int uid = Home.uid;
                    int type_trade = op;

                    string comando = "select balance from Account where uid = @uid";

                    SqlCommand cmd = con.CreateCommand();
                    cmd.Parameters.AddWithValue("@uid", uid);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = comando;

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        balance_antigo = reader.GetDouble(0);
                    }
                    con.Close();

                    string balance = gunaTextBox1.Text.Replace('.', ','); //para converter em double tem que estar com ,

                    double balance_novo = Convert.ToDouble(balance);


                    comando = "INSERT INTO Transactions(uid, id_type, tvalue, balance) VALUES(@uid, @type, @value, @bal)";
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand(comando, con);

                    cmd1.Parameters.AddWithValue("@uid", uid);
                    cmd1.Parameters.AddWithValue("@type", type_trade);
                    cmd1.Parameters.AddWithValue("@value", balance_novo);
                    cmd1.Parameters.AddWithValue("@bal", balance_antigo);
                    cmd1.CommandType = CommandType.Text;

                    try
                    {
                        cmd1.ExecuteNonQuery();
                        MessageBox.Show("Transação efetuada com sucesso!", "Ação finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                #endregion

                //refresh

                #region Refresh

                try
                {
                    reader = null;
                    try
                    {
                        double balance_antigo = 0;

                        con.Open();
                        string command = "SELECT name, balance, profile_photo, symbol_currency FROM Account, Currency WHERE Account.id_currency = Currency.id_currency AND uid = @uid";

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
                            label4.Text = (reader.GetString(0));

                            balance_antigo = (reader.GetDouble(1));

                            label3.Text = string.Format("{0:#,0.00}", balance_antigo) + reader.GetString(3);

                            if (reader.GetSqlBytes(2).Buffer != null)
                            {
                                MemoryStream str = new MemoryStream(reader.GetSqlBytes(2).Buffer);
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

                #endregion


                #region Tools Stuff

                gunaButton1.Visible = true;
                gunaButton2.Visible = true;
                gunaTextBox1.Visible = false;
                label1.Text = "Escolha a opção que prentende:";
                gunaCirclePictureBox1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                gunaButton3.Visible = false;
                gunaButton4.Visible = false;
                gunaButton5.Visible = false;
                gunaButton6.Visible = false;
                gunaButton7.Visible = false;
                gunaButton8.Visible = false;
                gunaButton9.Visible = false;
                gunaButton10.Visible = false; 
                gunaButton11.Visible = true;

                #endregion
            }
            else
            {
                if (op == 2) //Levantamento
                {
                    #region Tools Stuff


                    gunaButton9.Visible = false;
                    gunaButton10.Visible = false;

                    #endregion

                    //Update ao balance em Account

                    #region UPDATE Account

                    SqlDataReader reader = null;
                    try
                    {
                        con.Open();
                        double balance_antigo = 0;

                        int uid = Home.uid;

                        string comando = "select balance from Account where uid = @uid";

                        SqlCommand cmd = con.CreateCommand();
                        cmd.Parameters.AddWithValue("@uid", uid);
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = comando;

                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            balance_antigo = reader.GetDouble(0);
                        }
                        con.Close();

                        string balance = gunaTextBox1.Text.Replace('.', ','); //para converter em double tem que estar com ,
                        double balance_novo = balance_antigo - Convert.ToDouble(balance);

                        comando = "update Account set balance = @bal where uid = @uid";
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand(comando, con);

                        cmd1.Parameters.AddWithValue("@bal", balance_novo);
                        cmd1.Parameters.AddWithValue("@uid", uid);
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
                        if (con != null)
                        { con.Close(); }
                    }

                    #endregion

                    //adicionar as transações

                    #region INSERT Transactions

                    reader = null;

                    try
                    {
                        con.Open();
                        double balance_antigo = 0;

                        int uid = Home.uid;
                        int type_trade = op;

                        string comando = "select balance from Account where uid = @uid";

                        SqlCommand cmd = con.CreateCommand();
                        cmd.Parameters.AddWithValue("@uid", uid);
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = comando;

                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            balance_antigo = reader.GetDouble(0);
                        }
                        con.Close();

                        string balance = gunaTextBox1.Text.Replace('.', ','); //para converter em double tem que estar com ,

                        double balance_novo = Convert.ToDouble(balance);


                        comando = "INSERT INTO Transactions(uid, id_type, tvalue, balance) VALUES(@uid, @type, @value, @bal)";
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand(comando, con);

                        cmd1.Parameters.AddWithValue("@uid", uid);
                        cmd1.Parameters.AddWithValue("@type", type_trade);
                        cmd1.Parameters.AddWithValue("@value", balance_novo);
                        cmd1.Parameters.AddWithValue("@bal", balance_antigo);
                        cmd1.CommandType = CommandType.Text;

                        try
                        {
                            cmd1.ExecuteNonQuery();
                            MessageBox.Show("Transação efetuada com sucesso!", "Ação finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                    #endregion

                    //refresh

                    #region Refresh

                    try
                    {
                        reader = null;
                        try
                        {
                            double balance_antigo = 0;

                            con.Open();
                            string command = "SELECT name, balance, profile_photo, symbol_currency FROM Account, Currency WHERE Account.id_currency = Currency.id_currency AND uid = @uid";

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
                                label4.Text = (reader.GetString(0));

                                balance_antigo = (reader.GetDouble(1));

                                label3.Text = string.Format("{0:#,0.00}", balance_antigo) + reader.GetString(3);

                                if (reader.GetSqlBytes(2).Buffer != null)
                                {
                                    MemoryStream str = new MemoryStream(reader.GetSqlBytes(2).Buffer);
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

                    #endregion

                    #region Tools Stuff

                    gunaButton1.Visible = true;
                    gunaButton2.Visible = true;
                    gunaTextBox1.Visible = false;
                    label1.Text = "Escolha a opção que prentende:";
                    gunaCirclePictureBox1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    gunaButton3.Visible = false;
                    gunaButton4.Visible = false;
                    gunaButton5.Visible = false;
                    gunaButton6.Visible = false;
                    gunaButton7.Visible = false;
                    gunaButton8.Visible = false;
                    gunaButton9.Visible = false;
                    gunaButton10.Visible = false;
                    gunaButton11.Visible = true;

                    #endregion
                }
            }
        }

        private void gunaButton11_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            Stats_DW childForm = new Stats_DW();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            Parent.Controls.Add(childForm);
            Parent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
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
            }
        }
    }
}
