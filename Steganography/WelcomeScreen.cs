using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Steganography
{
    public partial class WelcomeScreen : MetroForm
    {
        public WelcomeScreen()
        {
            InitializeComponent();
           
        }

      

        private void WelcomeScreen_Shown(object sender, EventArgs e)
        {
            labelProgress.Text = "Loading Components";
            this.Refresh();
            Thread.Sleep(10);
            progressBar.Value = 10;
            labelProgress.Text = "Loading Components  20%";
            this.Refresh();
            Thread.Sleep(300);
            progressBar.Value = 20;
            labelProgress.Text = "Loading Components  30%";
            this.Refresh();
            Thread.Sleep(300);
            progressBar.Value = 30;
            labelProgress.Text = "Loading Components  40%";
            this.Refresh();
            Thread.Sleep(300);
            progressBar.Value = 40;
            labelProgress.Text = "Loading Components  50%";
            this.Refresh();
            Thread.Sleep(300);
            progressBar.Value = 50;
            labelProgress.Text = "Loading Components  60%";
            this.Refresh();
            Thread.Sleep(300);
            progressBar.Value = 60;
            this.Refresh();
            labelProgress.Text = "Setting Up  70%";
            this.Refresh();
            Thread.Sleep(300);            
            progressBar.Value = 80;
            labelProgress.Text = "Setting Up  80%";
            this.Refresh();
            Thread.Sleep(300);
            progressBar.Value = 90;
            labelProgress.Text = "Almost Done  90%";
            this.Refresh();
            Thread.Sleep(300);
            progressBar.Value = 100;
            labelProgress.Text = "Almost Done  100%";
            this.Refresh();
            Thread.Sleep(300);
            Steganography form = new Steganography();
            form.Show();
            this.Hide();


        }

        private void WelcomeScreen_Load(object sender, EventArgs e)
        {
            
            
        }
    }
}
