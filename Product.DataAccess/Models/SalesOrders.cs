using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Product.DataAccess.Models
{
    public class SalesOrders
    {
        public SalesOrders()
        {
            SOLines = new HashSet<SOLines>();
        }
        public int Id { get; set; }
        public string No { get; set; }
        public string CustomerName { get; set; }
        public string Discription { get; set; }
        public string SOStatus { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string PhoneNo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public ICollection<SOLines> SOLines { get; set; }


    }
}
