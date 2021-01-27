using Microsoft.AspNetCore.Mvc.Rendering;
using Product.DataAccess.CustomModel;
using Product.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Product.DataAccess.ViewModel
{
    public class ProductVM 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is Required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Code is Required")]
        public string Code { get; set; }
        public DateTime? InsertedDate { get; set; }
        public string StartDateFormatted { get; set; }
        [Required(ErrorMessage = "Category is Required")]
        public int CategoryId { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        // public virtual CategoryVM Category { get; set; }
    }
}
