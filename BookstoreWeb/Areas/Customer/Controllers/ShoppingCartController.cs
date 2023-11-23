using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models.Models;
using Bookstore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookstoreWeb.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class ShoppingCartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

		public ShoppingCartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartViewModel = new()
			{
				ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == userId,
				includeProperties: "Product")
			};

			foreach (var shoppingCart in ShoppingCartViewModel.ShoppingCartList)
			{
				shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart);
				ShoppingCartViewModel.OrderTotal += (shoppingCart.Price * shoppingCart.Count);
			}

			return View(ShoppingCartViewModel);
		}

		public IActionResult Plus(int cartId)
		{
			var shoppingCartFromDb = _unitOfWork.ShoppingCart.Get(s => s.Id == cartId);
			shoppingCartFromDb.Count += 1;
			_unitOfWork.ShoppingCart.Update(shoppingCartFromDb);
			_unitOfWork.Save();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Minus(int cartId)
		{
			var shoppingCartFromDb = _unitOfWork.ShoppingCart.Get(s => s.Id == cartId);

			if (shoppingCartFromDb.Count == 0)
				_unitOfWork.ShoppingCart.Remove(shoppingCartFromDb);
			else
				shoppingCartFromDb.Count -= 1;
			_unitOfWork.ShoppingCart.Update(shoppingCartFromDb);


			_unitOfWork.Save();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Remove(int cartId)
		{
			var shoppingCartFromDb = _unitOfWork.ShoppingCart.Get(s => s.Id == cartId);
			_unitOfWork.ShoppingCart.Remove(shoppingCartFromDb);
			_unitOfWork.Save();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Summary()
		{
			return View();
		}

		private decimal GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
		{
			if (shoppingCart.Count <= 50)
				return shoppingCart.Product.Price;
			else
			{
				if (shoppingCart.Count <= 100)
					return shoppingCart.Product.Price50;
				else
					return shoppingCart.Product.Price100;
			}
		}
	}
}
