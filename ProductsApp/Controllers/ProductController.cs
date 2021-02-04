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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ProductsApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public ProductController(IWebHostEnvironment env,IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexAjax([FromBody] ProductVM model)
        {
            var data0 = _productRepository.GetAllAsyncPage(model.PageNo, model.PageSize
                , c => c.Name.ToLower().Contains(model.SearchText));
            var data = data0.Item1.Include(c => c.Category).Select(v => new {
                    Id=v.Id,
                    Name = v.Name,
                    InsertedDate = v.InsertedDate,
                    Price = v.Price,
                    Code = v.Code,
                    ProdCat = v.Category.Name
                }).ToList();
          //  var modelVm = _mapper.Map<List<ProductVM>>(data0.Item1);

            return Json(new
            {
                TotalItems = data0.Item2,
                Data = data
            });
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

                String UniqueFileName = null;
                string FilePath = null;
                if (product.files != null)
                {
                    string FileNameUploader = Path.Combine(_env.WebRootPath, "ProductImages");
                    if (!Directory.Exists(FileNameUploader))
                    {
                        Directory.CreateDirectory(FileNameUploader);
                    }
                    // UniqueFileName = Guid.NewGuid().ToString() + "_" + product.files.FileName;
                    UniqueFileName = product.Id.ToString() + "_" + product.files.FileName;
                    FilePath = Path.Combine(FileNameUploader, UniqueFileName);
                    product.files.CopyTo(new FileStream(FilePath, FileMode.Create));
                    product.Image = UniqueFileName;

                }
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
                var test = _mapper.Map<Products>(product);
                test.Category = (await _categoryRepository.GetAsync(x => x.Id == product.CategoryId)).FirstOrDefault();

                await _productRepository.AddAsync(test);

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
                String UniqueFileName = null;
                String UniqueFileNameOld = null;
                string FilePath = null;
                string FilePathOld = null;
                
                // string oldfilePath = product.files.FileName;
                if (product.files != null)
                {
                    if (product.Image != null)
                    {
                        FilePathOld = Path.Combine(_env.WebRootPath, "ProductImages", product.Image);
                        if (System.IO.File.Exists(FilePathOld))
                        {
                            System.IO.File.Delete(FilePathOld);
                        }
                    }
                    string FileNameUploader = Path.Combine(_env.WebRootPath, "ProductImages");
                    if (!Directory.Exists(FileNameUploader))
                    {
                        Directory.CreateDirectory(FileNameUploader);
                    }
                   // UniqueFileName = Guid.NewGuid().ToString() + "_" + product.files.FileName;
                    UniqueFileName = product.Id.ToString() + "_" + product.files.FileName;
                    FilePath = Path.Combine(FileNameUploader, UniqueFileName);
                    
                    product.files.CopyTo(new FileStream(FilePath, FileMode.Create));
                    product.Image = UniqueFileName;

                }
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
                //  Products model = await _productRepository.FindAsync(product.Id).ConfigureAwait(false);
              var test = _mapper.Map<Products>(product);
              test.Category = (await _categoryRepository.GetAsync(x => x.Id == product.CategoryId)).FirstOrDefault();

                await _productRepository.UpdateAsync(test);
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
