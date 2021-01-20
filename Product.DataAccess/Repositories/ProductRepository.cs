using Product.DataAccess.Data;
using Product.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Product.DataAccess.Repositories
{
    public class ProductRepository : Repository<Products>, IProductRepository
    {

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Products> GetTopSellingProducts(int count)
        {
            return Context.Product.OrderByDescending(p => p.Price).Take(count).ToList();
        }
    }
}
