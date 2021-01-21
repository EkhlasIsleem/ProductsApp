using AutoMapper;
using Product.DataAccess.Models;
using Product.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApp.Extensions
{
    public class MapperProfileCollection : Profile
    {
        public MapperProfileCollection()
        {
            CreateMap<Products, ProductVM>().ReverseMap();
            CreateMap<Category, CategoryVM>().ReverseMap();
        }
    }
}
