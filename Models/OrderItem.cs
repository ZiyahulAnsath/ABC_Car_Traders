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
            public string CustomerName { get; set; }  
            public int ProductID { get; set; }
            public string ProductName { get; set; }   
            public string Model { get; set; }         
            public string Brand { get; set; }         
            public decimal Price { get; set; }
            public DateTime OrderDate { get; set; }
            public int Status { get; set; }
    }
}
