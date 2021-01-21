using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Product.DataAccess.Data;
using Product.DataAccess.Models;
using Product.DataAccess.Repositories;
using Product.DataAccess.ViewModel;
using AutoMapper;

namespace ProductsApp.Controllers
{
   
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> IndexAjax()
        {
            return Json(_mapper.Map<List<CategoryVM>>(await _categoryRepository.GetAllAsync()));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<CategoryVM>(category));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM category)
        {
            if (ModelState.IsValid)
            {
               await _categoryRepository.AddAsync(_mapper.Map<Category>(category));
               return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var category = await _categoryRepository.FindAsync(id);
            return View(_mapper.Map<CategoryVM>(category));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryVM category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                 int result = await _categoryRepository.UpdateAsync(_mapper.Map<Category>(category));
              
                if(result > 0)
                   return RedirectToAction(nameof(Index));
                else
                    return View(category);
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<CategoryVM>(category));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.FindAsync(id);
            await _categoryRepository.RemoveAsync(category);
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Delete(int id)
        //{
        //    var model = await _categoryRepository.FindAsync(id).ConfigureAwait(false);

        //    await _categoryRepository.RemoveAsync(model).ConfigureAwait(false);
        //    return Json(new
        //    {
        //        status = 1,
        //        link = "",
        //        management = "Category",
        //        msg = "Category was deleted successfully form the database."
        //    });
        //}

    }
}
