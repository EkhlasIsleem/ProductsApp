using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.DataAccess.ViewModel
{
    public class SalesOrdersVM
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string CustomerName { get; set; }
        public string Discription { get; set; }
        public string SOStatus { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string PhoneNo { get; set; }
        public decimal Amount { get; set; }
        public string SearchNo { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

    }
}
