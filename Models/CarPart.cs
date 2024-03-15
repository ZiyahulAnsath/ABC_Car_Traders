using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Car_Traders.Models
{
    public class CarPart
    {
        public int PartID { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string PartName { get; set; }
        public string Description { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public DateTime Date { get; set; }
        public int QuantityAvailable { get; set; }
        public byte[] Image { get; set; }
    }

}
