using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models.Models;
using Bookstore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace BookstoreWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public ProductController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			List<Product> productList = _unitOfWork.Product.GetAll().ToList();
			
			return View(productList);
		}

		[HttpGet]
		public IActionResult Upsert(int? id)
		{
			ProductViewModel productViewModel = new()
			{
				CategoryList = _unitOfWork.Category
				.GetAll().Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString()
				}),
				Product = new Product()
			};

			if(id.Equals(null) || id == 0)
			{
				// Create.
				return View(productViewModel);
			}
			else
			{
				productViewModel.Product = _unitOfWork.Product.Get(p => p.Id == id);
				return View(productViewModel); 
			}
		}

		[HttpPost]
		public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
		{
			// Check input validation
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Add(productViewModel.Product);
				_unitOfWork.Save();
				TempData["success"] = "Product created seccessfully!";
				return RedirectToAction("Index", "Product");
			}
			else
			{
				productViewModel.CategoryList = _unitOfWork.Category
				.GetAll().Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString()
				});
				return View(productViewModel);
			}
		}

		[HttpGet]
		public IActionResult Delete(int? id) 
		{
			var product = _unitOfWork.Product.Get(p => p.Id == id);

			if(product == null)
				return NotFound();

			return View(product);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			var product = _unitOfWork.Product.Get(p => p.Id == id);

			if (product == null)
				return NotFound();

			_unitOfWork.Product.Remove(product);
			_unitOfWork.Save();
			TempData["success"] = "Product deleted seccessfully!";
			return RedirectToAction("Index", "Product");
		}
	}
}
