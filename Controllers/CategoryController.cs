using ConsoleData.DTO;
using ControleContasData.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.Data.Domain;
using Shared.Data.Repositories.Interfaces;

namespace ControleContas.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryRepository _categoryRepository;
		public CategoryController(ICategoryRepository categoryRepository) 
		{
			_categoryRepository = categoryRepository;
		}
		public IActionResult Index()
		{
			var listaCategorias = _categoryRepository.GetAll();
			return View(listaCategorias);
		}

        public IActionResult Edit(int id)
        {
            var category = _categoryRepository.GetById(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {                
                TimeZoneInfo fusoHorarioBrasil = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                _categoryRepository.Update(category);
                return RedirectToAction("Index");
            }
            return View(category);

        }
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(Category category)
        {
            if (ModelState.IsValid)
            {
                
                _categoryRepository.Save(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Delete(int id)
        {            
            _categoryRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
