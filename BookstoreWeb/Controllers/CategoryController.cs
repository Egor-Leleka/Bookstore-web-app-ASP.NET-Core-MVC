using Bookstore.DataAccess.Data;
using Bookstore.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace BookstoreWeb.Controllers
{
	public class CategoryController : Controller
	{
		private readonly Entities _entities;
		public CategoryController(Entities entities)
		{
			_entities = entities;
		}

		public IActionResult Index()
		{
			List<Category> categoryList = _entities.Categories.ToList();
			return View(categoryList);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Category category)
		{
			// Check input validation
			if (ModelState.IsValid)
			{
				_entities.Categories.Add(category);
				_entities.SaveChanges();
				TempData["success"] = "Category created seccessfully!";
				return RedirectToAction("Index", "Category");
			}
			return View();
		}

		[HttpGet]
		public IActionResult Edit(int? id)
		{
			var category = _entities.Categories.FirstOrDefault(c => c.Id == id);

			if (category == null)
				return NotFound();

			return View(category);
		}
		
		[HttpPost]
		public IActionResult Edit(Category category)
		{
			// Check input validation
			if (ModelState.IsValid)
			{
				_entities.Categories.Update(category);
				_entities.SaveChanges();
				TempData["success"] = "Category updated seccessfully!";
				return RedirectToAction("Index", "Category");
			}
			return View();
		}

		[HttpGet]
		public IActionResult Delete(int? id)
		{
			var category = _entities.Categories.FirstOrDefault(c => c.Id == id);

			if (category == null)
				return NotFound();

			return View(category);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			var category = _entities.Categories.FirstOrDefault(c => c.Id == id);

			if (category == null)
				return NotFound();

			_entities.Categories.Remove(category);
			_entities.SaveChanges();
			TempData["success"] = "Category deleted seccessfully!";
			return RedirectToAction("Index", "Category");
		}
	}
}
