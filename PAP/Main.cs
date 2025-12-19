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
    public partial class Main : Form
    {
        Thread t1;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            
        }

        private void btregister_Click(object sender, EventArgs e)
        {
            this.Close();
            t1 = new Thread(abrirjanela_register);
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
        }

        private void btlogin_Click(object sender, EventArgs e)
        {
            this.Close();
            t1 = new Thread(abrirjanela_login);
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
        }

        private void abrirjanela_register(object obj)
        {
            Application.Run(new Register());
        }

        private void abrirjanela_login(object obj)
        {
            Application.Run(new Login());
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

        private void gunaControlBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
