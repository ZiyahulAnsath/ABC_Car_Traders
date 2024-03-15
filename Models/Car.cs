using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ABC_Car_Traders.Models
{
    public class Car
    {
        public int CarID { get; set; }
        public string CarName { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string FualType { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public DateTime Date { get; set; }
        public byte[] Image { get; set; }
        public int QuantityAvailable { get; set; }
    }
}
