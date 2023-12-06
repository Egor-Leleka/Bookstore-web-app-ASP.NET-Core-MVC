using Bookstore.DataAccess.Data;
using Bookstore.Models.Models;
using Bookstore.Models.ViewModels;
using Bookstore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookstoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class UserController : Controller
    {
        private readonly Entities _entities;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(Entities entities, UserManager<IdentityUser> userManager)
        {
            _entities = entities;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RoleManagment(string userId)
        {
            string roleId = _entities.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;

            RoleManagmentViewModel roleVM = new RoleManagmentViewModel()
            {
                ApplicationUser = _entities.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
                RoleList = _entities.Roles.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Name
                }),
                CompanyList = _entities.Companies.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            roleVM.ApplicationUser.Role = _entities.Roles.FirstOrDefault(u => u.Id == roleId).Name;

            return View(roleVM);
        }

        [HttpPost]
        public IActionResult RoleManagment(RoleManagmentViewModel roleVm)
        {
            string roleId = _entities.UserRoles.FirstOrDefault(u => u.UserId == roleVm.ApplicationUser.Id).RoleId;
            string oldRole = _entities.Roles.FirstOrDefault(u => u.Id == roleId).Name;

            if (!(roleVm.ApplicationUser.Role == oldRole))
            {
                ApplicationUser applicationUser = _entities.ApplicationUsers
                    .FirstOrDefault(u => u.Id == roleVm.ApplicationUser.Id);

                if (roleVm.ApplicationUser.Role == StaticDetails.Role_Company)
                    applicationUser.CompanyId = roleVm.ApplicationUser.CompanyId;

                if (oldRole == StaticDetails.Role_Company)
                    applicationUser.CompanyId = null;

                _entities.SaveChanges();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleVm.ApplicationUser.Role).GetAwaiter().GetResult();
            }

            return RedirectToAction("Index");
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> userList = _entities.ApplicationUsers.Include(u => u.Company).ToList();
            var userRoles = _entities.UserRoles.ToList();
            var roles = _entities.Roles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if (user.Company == null)
                {
                    user.Company = new Company { Name = "" };

                }
            }

            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var user = _entities.ApplicationUsers.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return Json(new { sucsess = false, message = "Error while Locking/Unlicking user" });

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
                user.LockoutEnd = DateTime.Now; // Unlock locked user.
            else
                user.LockoutEnd = DateTime.Now.AddYears(100); // Lock user for 100 years.

            _entities.SaveChanges();

            return Json(new { success = true, message = "User Locked/UnLocked successfully" });
        }


        #endregion


    }
}
