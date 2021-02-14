using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.DataAccess.ViewModel
{
    public class SOLinesVM
    {
        public int Id { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public decimal Price { get; set; }
        public int SalesOrderId { get; set; }
    }
}
