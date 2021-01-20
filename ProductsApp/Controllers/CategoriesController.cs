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

namespace ProductsApp.Controllers
{
   
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> IndexAjax()
        {
            return Json(await _categoryRepository.GetAllAsync());
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

            return View(category);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
               await _categoryRepository.AddAsync(category);
               return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var category = await _categoryRepository.FindAsync(id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                 int result = await _categoryRepository.UpdateAsync(category);
              
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

            return View(category);
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
