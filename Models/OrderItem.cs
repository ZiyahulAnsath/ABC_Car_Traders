using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Car_Traders.Models
{
    public class OrderItem
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; } 
        public int CustomerName { get; set; } 
        public int ProductID { get; set; } 
        public int ProductName { get; set; }
        public int Model { get; set; }
        public int Brand { get; set; }
        public int Price { get; set; }
        public int OrderDate { get; set; }
        public int Status { get; set; }
    }
}
