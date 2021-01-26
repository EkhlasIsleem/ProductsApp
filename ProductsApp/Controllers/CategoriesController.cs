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
using Product.DataAccess.Enum;
using Product.DataAccess.Base.Enum;

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
            var res = 0;
            if (!string.IsNullOrEmpty(category.Name))
            {
                var isExist = _categoryRepository.Get(c => c.Name == category.Name).Count() > 0;
                if (isExist)
                   return View(category);
                //res = -1;
                //return res;
            }
            if (ModelState.IsValid)
            {
               await _categoryRepository.AddAsync(_mapper.Map<Category>(category));
                 return RedirectToAction(nameof(Index));
                //res = 1;
                //return res;
            }
            //return res;
             return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var category = await _categoryRepository.FindAsync(id);
            return View(_mapper.Map<CategoryVM>(category));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryVM category)
        {
          
            if (!string.IsNullOrEmpty(category.Name))
            {
                var isExist = (await _categoryRepository.GetAsync(c => c.Name.ToLower() == category.Name.ToLower() && c.Id != category.Id).ConfigureAwait(false)).Any();
                if (isExist)
                    // return View(category);
                    return Json(new
                    {
                        status = JsonStatus.Exist,
                        link = "",
                        color = NotificationColor.Error.ToColorName(),
                        management = "Category Management",
                        msg = "Category is exist.",
                        editResult = category
                    });
            }
            if (ModelState.IsValid)
            {
                 await _categoryRepository.UpdateAsync(_mapper.Map<Category>(category));
                // return RedirectToAction(nameof(Index));
                return Json(new
                {
                    status = JsonStatus.Success,
                    link = "",
                    color = NotificationColor.Success.ToColorName(),
                    management = "Category Management",
                    msg = "Category was updated successfully into the database.",
                    editResult = category
                });
            }

            return View(category);
        }
        public async Task<IActionResult> _Edit(int id)
        {
            var model = await _categoryRepository.FindAsync(id).ConfigureAwait(false);
            var modelVm = _mapper.Map<CategoryVM>(model);
            
            return PartialView(modelVm);
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
