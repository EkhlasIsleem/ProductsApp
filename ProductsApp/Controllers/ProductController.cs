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


        public async Task<IActionResult> Details(int? id)
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

            return View(_mapper.Map<ProductVM>(product));
        }


        public IActionResult Create(bool isSuccess)
        {
            ViewBag.IsSuccess = isSuccess;
            return View(new ProductVM
            {
                CategoryList = _categoryRepository.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }).ToList()
            });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM product)
        {
           // product.InsertedDate = DateTime.Now.;
            product.StartDateFormatted = DateTime.Now.ToShortDateString();
          //  product.InsertedDate = DateTime.Parse(product.StartDateFormatted, CultureInfo.InvariantCulture); 
            /*product.InsertedDate = new DateTime(Convert.ToInt32(product.StartDateFormatted.Substring(4, 4)), // Year
                                    Convert.ToInt32(product.StartDateFormatted.Substring(2, 2)), // Month
                                    Convert.ToInt32(product.StartDateFormatted.Substring(0, 2)));// Day*/
            product.InsertedDate = DateTime.ParseExact(product.StartDateFormatted, "M/dd/yyyy", CultureInfo.InvariantCulture); 
            if (!string.IsNullOrEmpty(product.Name))
            {
                var isExist = (await _categoryRepository.GetAsync(c => c.Name.ToLower() == product.Name.ToLower()).ConfigureAwait(false)).Any();
                if (isExist)
                    return View(product);
            }
            //if (ModelState.IsValid)
            //{
               int id= await _productRepository.AddAsync(_mapper.Map<Products>(product));
                //return RedirectToAction(nameof(Create), new { isSuccess = true });
                if (id > 0)
                  {
                   return RedirectToAction(nameof(Create), new { isSuccess = true });
                  }else
           // }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
           // ViewBag.categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(new ProductVM
            {
                CategoryList = _categoryRepository.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }).ToList()
            });
           // return View(_mapper.Map<ProductVM>(product));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductVM product)
        {

            if (id != product.Id)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(product.Name))
            {
                var isExist = (await _categoryRepository.GetAsync(c => c.Name.ToLower() == product.Name.ToLower() && c.Id != product.Id).ConfigureAwait(false)).Any();
                if (isExist)
                    return View(product);
            }
            if (ModelState.IsValid)
            {

                await _productRepository.UpdateAsync(_mapper.Map<Products>(product));
                return RedirectToAction(nameof(Index));

                /*if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                    return View(product);*/
             }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
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

            return View(_mapper.Map<ProductVM>(product));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.FindAsync(id);
            await _productRepository.RemoveAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
