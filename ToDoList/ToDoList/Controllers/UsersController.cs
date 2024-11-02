using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string search)
        {
            var users = new List<ApplicationUser>();
            if (search != null)
            {
                 users = await _context.Users.Where(x => x.Email != "orxanm385@gmail.com").Where(x => x.EmailConfirmed && x.FirstName.Contains(search)).ToListAsync();

            }
            else
            {
                 users = await _context.Users.Where(x => x.Email != "orxanm385@gmail.com").Where(x => x.EmailConfirmed).ToListAsync();

            }

            var vm = users.Select(x => new UserVM
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                RegisterDate = x.RegisterDate,
                Username = x.UserName,
                Email = x.Email,
            }).ToList();
            return View(vm);
        }
    }
}
