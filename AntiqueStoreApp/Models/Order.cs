using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiqueStoreApp.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerFullName { get; set; }
        public decimal TotalPrice { get; set; } 
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

}
