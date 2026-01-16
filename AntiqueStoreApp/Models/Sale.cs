using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiqueStoreApp.Models
{
    public class Sale
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
   
        public DateTime Date { get; set; }

        public string CustomerFullName { get; set; }
        public string ProductName { get; set; }
    }

}
