using Product.DataAccess.Data;
using Product.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Product.DataAccess.Repositories
{
    public class SalesOrdersRepository : Repository<SalesOrders>, ISalesOrdersRepository
    {

        public SalesOrdersRepository(ApplicationDbContext context) : base(context)
        {
        }

     
    }
}
