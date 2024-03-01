using System;
using System.Windows.Forms;
using ABC_Car_Traders.Views.Admin;

namespace ABC_Car_Traders
{
    public partial class LoadingPage : Form
    {
        public LoadingPage()
        {
            InitializeComponent();
            tLoading.Start();
        }

        private void tLoading_Tick(object sender, EventArgs e)
        {
            tLoading.Enabled = true;
            guna2ProgressBar1.Increment(5);
            if(guna2ProgressBar1.Value == 100)
            {
                tLoading.Enabled = false;
                AdminLoginForm login = new AdminLoginForm();
                login.Show();
                this.Hide();
            }
        }

       
    }
}
