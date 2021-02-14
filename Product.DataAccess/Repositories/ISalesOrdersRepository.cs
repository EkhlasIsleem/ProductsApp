using Product.DataAccess.Models;
using Product.DataAccess.ViewModel;
using Product.DataAccess.Repositories;
using System.Collections.Generic;

namespace Product.DataAccess.Repositories
{
    public interface ISalesOrdersRepository : IRepository<SalesOrders>
    {
    }
}
