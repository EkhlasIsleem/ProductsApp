using Product.DataAccess.Models;
using Product.DataAccess.Repositories;
using System.Collections.Generic;

namespace Product.DataAccess.Repositories
{
    public interface IProductRepository : IRepository<Products>
    {
        IEnumerable<Products> GetTopSellingProducts(int count);
    }
}
