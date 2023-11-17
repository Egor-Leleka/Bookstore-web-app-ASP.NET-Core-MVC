using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models.Models;
using Bookstore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookstoreWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

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

			if (id == null || id == 0)
			{
				// Create.
				return View(productViewModel);
			}
			else
			{
				// Update.
				productViewModel.Product = _unitOfWork.Product.Get(p => p.Id == id);
				return View(productViewModel);
			}
		}

		[HttpPost]
		public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string productPath = Path.Combine(wwwRootPath, @"images\product");

					if (!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
					{
						// Delete old image.
						var oldImagePath =
							Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.TrimStart('\\'));

						if (System.IO.File.Exists(oldImagePath))
							System.IO.File.Delete(oldImagePath);
					}

					using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					productViewModel.Product.ImageUrl = @"\images\product\" + fileName;
				}

				if (productViewModel.Product.Id == 0)
				{
					_unitOfWork.Product.Add(productViewModel.Product);
				}
				else
				{
					_unitOfWork.Product.Update(productViewModel.Product);
				}

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

		// Using API for deleting.
		/*
		[HttpGet]
		public IActionResult Delete(int? id)
		{
			var product = _unitOfWork.Product.Get(p => p.Id == id);

			if (product == null)
				return NotFound();

			return View(product);
		}
		*/

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

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

			return Json(new { data = productList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var productToBeDeleted = _unitOfWork.Product.Get(p => p.Id == id);
			if (productToBeDeleted == null)
				return Json(new { success = false, message = "Error occured while deleting" });

			var oldImagePath =
							Path.Combine(_webHostEnvironment.WebRootPath,
							productToBeDeleted.ImageUrl.TrimStart('\\'));

			if (System.IO.File.Exists(oldImagePath))
				System.IO.File.Delete(oldImagePath);

			_unitOfWork.Product.Remove(productToBeDeleted);
			_unitOfWork.Save();
			return Json(new { success = true, message = "Product deleted successfully" });
		}


		#endregion
	}
}
