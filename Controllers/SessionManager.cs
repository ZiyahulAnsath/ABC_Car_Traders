using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABC_Car_Traders.Models;

namespace ABC_Car_Traders.Controllers
{
    public static class SessionManager
    {
        public static int LoggedInCustomerId { get; set; }
        public static string LoggedInCustomerName { get; set; }
    }
}
