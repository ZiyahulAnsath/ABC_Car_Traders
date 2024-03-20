using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABC_Car_Traders.Views.Admin;
using ABC_Car_Traders.Views.Customer;

namespace ABC_Car_Traders
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //string connectionString = "Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True";
            //Application.Run(new LoadingPage(connectionString));
            Application.Run(new LoadingPage());
        }
    }
}
