using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager)

        {

            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;

        }
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var vm = new UserVM()
            {
                Id = loggedInUser.Id,
                FirstName = loggedInUser.FirstName,
                LastName = loggedInUser.LastName,
                Username = loggedInUser.UserName,
                Email = loggedInUser.Email,
                RegisterDate = loggedInUser.RegisterDate,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var result = await _userManager.DeleteAsync(loggedInUser);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Your account has been succesfully deleted";
                await _signInManager.SignOutAsync();
                return RedirectToAction("Home","Pages");
            }
            return View();

        }
    }
}
