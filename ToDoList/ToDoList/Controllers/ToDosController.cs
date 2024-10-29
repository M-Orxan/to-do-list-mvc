using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
    public class ToDosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var toDos = await _context.ToDos.ToListAsync();
            var toDoVMs = toDos.Select(x => new ToDoVM
            {
                Title = x.Title,
                Deadline = x.Deadline,
                Description = x.Description,
                ApplicationUserId = x.ApplicationUserId,
                CreatedDate = x.CreatedDate,
                IsCompleted = x.IsCompleted,
            }).ToList();

            return View(toDoVMs);
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateToDoVM vm)
        {
            if (vm.Title == null)
            {
                ModelState.AddModelError("Title", "Title field is required");
                return View(vm);
            }


            var checkByTitle=await _context.ToDos.AnyAsync(x=> x.Title == vm.Title);
            if (checkByTitle)
            {
                ModelState.AddModelError("Title", "To do with the same title already exists");
                return View(vm);
            }

            if (vm.Deadline < DateTime.Now.AddDays(-1))
            {
                ModelState.AddModelError("Deadline", "The deadline cannot be in the past");
                return View(vm);
            }
            var loggedInUser = await _userManager.GetUserAsync(User);

            var toDo = new ToDo()
            {
                Title = vm.Title,
                Deadline = vm.Deadline,
                Description = vm.Description,
                ApplicationUserId = loggedInUser.Id,
                CreatedDate = DateTime.Now,
                IsCompleted = false,
            };

            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ToDos");
        }
    }
}
