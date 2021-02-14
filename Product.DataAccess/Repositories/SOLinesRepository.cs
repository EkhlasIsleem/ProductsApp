using Product.DataAccess.Data;
using Product.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Product.DataAccess.Repositories
{
    public class SOLinesRepository : Repository<SOLines>, ISOLinesRepository
    {

        public SOLinesRepository(ApplicationDbContext context) : base(context)
        {
        }

      
    }
}
