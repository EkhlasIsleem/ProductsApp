using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Product.DataAccess.ViewModel
{
    public class CategoryVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }

    }
}
