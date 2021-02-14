using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product.DataAccess.Models;

namespace Product.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Products> Product { get; set; }
        public DbSet<SalesOrders> SalesOrders { get; set; }
        public DbSet<SOLines> SOLines { get; set; }
    
    }
}
