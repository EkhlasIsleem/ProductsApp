using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Product.DataAccess.Repositories;
using Product.DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Product.DataAccess.ViewModel;
using AutoMapper;
using System.Globalization;
using Product.DataAccess.Enum;
using Product.DataAccess.Base.Enum;
using Microsoft.EntityFrameworkCore;

namespace ProductsApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexAjax([FromBody] ProductVM model)
        {
            var count = _productRepository.GetAllAsyncPage(model.PageNo, model.PageSize
                , c => c.Name.ToLower().Contains(model.SearchText)).Item2;
            var data = _productRepository.GetAllAsyncPage(model.PageNo, model.PageSize
                , c => c.Name.ToLower().Contains(model.SearchText)).Item1.Include(c => c.Category).Select(v => new {
                    Name = v.Name,
                    InsertedDate = v.InsertedDate,
                    Price = v.Price,
                    Code = v.Code,
                    ProdCat = v.Category.Name
                }).ToList(); 
            // return Json(data.Item1);
           // var modelVm = _mapper.Map<IQueryable<ProductVM>>(data);

           
            return Json(new
            {
                TotalItems = count,
                Data = data
        });
        }
        public IActionResult TestData()
        {
            return Json(_productRepository.GetAllQuerable().Include(c => c.Category).Select(v => new {
                ProName = v.Name,
                ProdCat = v.Category.Name
            }).ToList());
        }
        public async Task<IActionResult> _Details(int? id)
        {
            var product = await _productRepository.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return PartialView(_mapper.Map<ProductVM>(product));
        }

        public IActionResult _Create()
        {
            var model = new ProductVM
            {
                CategoryList = _categoryRepository.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }).ToList()
            };
            return PartialView(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM product)
        {
            product.InsertedDate = DateTime.Now.Date;
            // product.StartDateFormatted = DateTime.ParseExact(product.StartDateFormatted, "MM/dd/yyyy", CultureInfo.InvariantCulture); 
            if (ModelState.IsValid)
            {
                var isExist = (await _productRepository.GetAsync(c => c.Name.ToLower() == product.Name.ToLower()).ConfigureAwait(false)).Any();
                if (isExist)
                    return Json(new
                    {
                        status = JsonStatus.Exist,
                        link = "",
                        color = NotificationColor.Error.ToColorName(),
                        management = "product Management",
                        msg = "product is exist.",
                        editResult = product
                    });           
            await _productRepository.AddAsync(_mapper.Map<Products>(product));

                return Json(new
                {
                    status = JsonStatus.Success,
                    link = "",
                    color = NotificationColor.Success.ToColorName(),
                    management = "product Management",
                    msg = "product was updated successfully into the database.",
                    editResult = product
                });
            }
            return Json(new
            {
                status = JsonStatus.Exist,
                link = "",
                color = NotificationColor.Error.ToColorName(),
                management = "product Management",
                msg = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                                    .Select(m => m.ErrorMessage).ToArray()
            });
        }

        public async Task<IActionResult> _Edit(int id)
        {

            var product = await _productRepository.FindAsync(id);
            if (product == null)
            {
                return Json(new
                {
                    status = JsonStatus.Exist,
                    link = "",
                    color = NotificationColor.Error.ToColorName(),
                    management = "product Management",
                    msg = "product Not found."
                });
            }
            var modelVm = _mapper.Map<ProductVM>(product);

            modelVm.CategoryList= 
                 _categoryRepository.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }).ToList();
            return PartialView(modelVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductVM product)
        {
            if (ModelState.IsValid)
            {
                var isExist = (await _productRepository.GetAsync(c => c.Name.ToLower() == product.Name.ToLower() && c.Id != product.Id).ConfigureAwait(false)).Any();
                if (isExist)
                    // return View(category);
                    return Json(new
                    {
                        status = JsonStatus.Exist,
                        link = "",
                        color = NotificationColor.Error.ToColorName(),
                        management = "product Management",
                        msg = "product is exist."
                    });
                Products model = await _productRepository.FindAsync(product.Id).ConfigureAwait(false);
                await _productRepository.UpdateAsync(_mapper.Map<Products>(product));
                return Json(new
                {
                    status = JsonStatus.Success,
                    link = "",
                    color = NotificationColor.Success.ToColorName(),
                    management = "product Management",
                    msg = "product was updated successfully into the database."
                });
            }

            return Json(new
            {
                status = JsonStatus.Exist,
                link = "",
                color = NotificationColor.Error.ToColorName(),
                management = "product Management",
                msg = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                         .Select(m => m.ErrorMessage).ToArray()
            });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _productRepository.FindAsync(id).ConfigureAwait(false);
            await _productRepository.RemoveAsync(model).ConfigureAwait(false);

            return Json(new
            {
                status = JsonStatus.Success,
                link = "",
                color = NotificationColor.Success.ToColorName(),
                management = "category",
                msg = "category was deleted successfully form the database."
            });
        }

    }
}
