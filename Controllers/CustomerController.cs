using ABC_Car_Traders.DataAccess;
using ABC_Car_Traders.Models;
using System.Collections.Generic;

namespace ABC_Car_Traders.Controllers
{
    class CustomerController
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerController()
        {
            _customerRepository = new CustomerRepository();
        }

        public void RegisterCustomer(Customer customer)
        {
            _customerRepository.InsertCustomer(customer);
        }

        public bool ValidateCustomerLogin(string username, string password)
        {
            return _customerRepository.ValidateCustomerLogin(username, password);
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }

        public int GetCustomerIdByUsername(string username)
        {
            return _customerRepository.GetCustomerIdByUsername(username);
        }

        public string GetCustomerNameById(int customerId)
        {
            return _customerRepository.GetCustomerNameById(customerId);
        }
    }
}
