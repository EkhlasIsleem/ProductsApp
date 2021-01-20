using System;
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

        [Display(Name = "Product Name")]
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public string Code { get; set; }
        public DateTime InsertedDate { get; set; }

        [Display(Name = "Category")]
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [Required]
        public virtual Category Category { get; set; }
    }
}
