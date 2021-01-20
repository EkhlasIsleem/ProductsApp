using Product.DataAccess.Data;
using Product.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Product.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

     
    }
}
