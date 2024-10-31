using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Utilities;
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(vm.Email);


            if (user == null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(vm);
            }

            var singInResult = await _signInManager.PasswordSignInAsync(user, vm.Password, true, false);

            if (!singInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(vm);
            }


            return RedirectToAction("Home", "Pages");
        }


        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var checkByEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (checkByEmail != null)
            {
                ModelState.AddModelError("Email", "This email aready exists");
                return View(vm);
            }

            //var checkByUsername = await _userManager.FindByNameAsync(vm.Email);
            //if (checkByUsername != null)
            //{
            //    ModelState.AddModelError("Username", "This username aready exists");
            //    return View(vm);
            //}

            var user = new ApplicationUser()
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                UserName = vm.Username,

            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, WebsiteRole.WebsiteUser);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }

            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Home", "Pages");
        }


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "Pages");
        }
    }
}
