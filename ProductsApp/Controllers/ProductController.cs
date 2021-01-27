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

       /* public async Task<IActionResult> IndexAjax()
        {
            return Json(_mapper.Map<List<ProductVM>>(await _productRepository.GetAllAsync()).Take(3));
        } */
        public IActionResult IndexAjax()
        {
            var data = _productRepository.GetAllAsyncPage(0, 3);
            return Json(data.Item1);
        }


        public async Task<IActionResult> _Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                if (!string.IsNullOrEmpty(product.Name))
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
            }
           
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

            if (id == null)
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
            var model = await _productRepository.FindAsync(id).ConfigureAwait(false);
            var modelVm = _mapper.Map<ProductVM>(model);

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
        public async Task<IActionResult> Edit(int id, ProductVM product)
        {
            product.InsertedDate = DateTime.Now.Date;

            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(product.Name))
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
                            msg = "product is exist.",
                            editResult = product
                        });
                }

                await _productRepository.UpdateAsync(_mapper.Map<Products>(product));
                // return RedirectToAction(nameof(Index));
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
