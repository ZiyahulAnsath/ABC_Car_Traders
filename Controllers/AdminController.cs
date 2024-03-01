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

        public AdminController(string connectionString)
        {
            adminRepository = new AdminRepository(connectionString);
        }

        public bool ValidateAdminLogin(string username, string password)
        {
            return adminRepository.ValidateAdminLogin(username, password);
        }
    }
}
