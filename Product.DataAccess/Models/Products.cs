﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Product.DataAccess.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public double Price { get; set; }
        public string Code { get; set; }
        public DateTime InsertedDate { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
