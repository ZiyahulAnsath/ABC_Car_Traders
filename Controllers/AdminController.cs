using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABC_Car_Traders.DataAccess;
using ABC_Car_Traders.Models;

namespace ABC_Car_Traders.Controllers
{
    public class AdminController
    {
        private AdminRepository adminRepository;

        public AdminController()
        {
            adminRepository = new AdminRepository();
        }

        public bool ValidateAdminLogin(string username, string password)
        {
            return adminRepository.ValidateAdminLogin(username, password);
        }
    }
}
