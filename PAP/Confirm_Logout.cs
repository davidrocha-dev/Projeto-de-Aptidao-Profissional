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

namespace PAP
{
    public partial class Confirm_Logout : Form
    {
        Thread t1;

        public Confirm_Logout()
        {
            InitializeComponent();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            t1 = new Thread(abrirjanela_main);
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
            Home.username = "";
            Home.uid = 0;
            this.Close();
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            t1 = new Thread(abrirjanela_home);
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
            this.Close();
        }

        private void abrirjanela_main(object obj)
        {
            Application.Run(new Main());
        }

        private void abrirjanela_home(object obj)
        {
            Application.Run(new Home());
        }

        private void gunaPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
