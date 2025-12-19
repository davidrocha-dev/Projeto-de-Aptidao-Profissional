using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace PAP
{

    public partial class HomeForm : Form
    {
        DateTime dt;

        public HomeForm()
        {
            InitializeComponent();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            label_time.Text = DateTime.Now.ToLongTimeString();
            label_date.Text = DateTime.Now.ToLongDateString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dt = DateTime.UtcNow;

            label_time.Text = DateTime.Now.ToLongTimeString();
            label_date.Text = DateTime.Now.ToLongDateString();

            if (dt.DayOfWeek == DayOfWeek.Friday && dt.Hour > 22)
            {
                label2.Text = "Mercado Fechado!";
                label2.ForeColor = System.Drawing.Color.Red;
            }else
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    label2.Text = "Mercado Fechado!";
                    label2.ForeColor = System.Drawing.Color.Red;
                }else
                {
                    if (dt.DayOfWeek == DayOfWeek.Sunday && dt.Hour < 22)
                    {
                        label2.Text = "Mercado Fechado!";
                        label2.ForeColor = System.Drawing.Color.Red;
                    }else
                    {
                        label2.Text = "Mercado Aberto!";
                        label2.ForeColor = System.Drawing.Color.LimeGreen;
                    }
                }
            }

            timer1.Start();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label_time_Click(object sender, EventArgs e)
        {

        }
    }
}
