using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Product.DataAccess.Models
{
    public class SOLines
    {
        public int Id { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int SalesOrderId { get; set; }
        public SalesOrders SalesOrders { get; set; }
    }
}
