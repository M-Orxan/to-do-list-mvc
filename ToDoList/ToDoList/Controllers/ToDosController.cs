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
    [Authorize(Roles ="User")]
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
            var loggedInUser = await _userManager.GetUserAsync(User);
            var toDos = await _context.ToDos.Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == loggedInUser.Id).ToListAsync();
            var toDoVMs = toDos.Select(x => new ToDoVM
            {
                Id = x.Id,
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

            var loggedInUser = await _userManager.GetUserAsync(User);
            var checkByTitle = await _context.ToDos.Where(x => x.ApplicationUserId == loggedInUser.Id).AnyAsync(x => x.Title == vm.Title);
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


            var toDo = new ToDo()
            {
                Id = vm.Id,
                Title = vm.Title,
                Deadline = vm.Deadline,
                Description = vm.Description,
                ApplicationUserId = loggedInUser.Id,
                CreatedDate = DateTime.Now,

            };

            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ToDos");
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ToDos");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            var toDoVM = new UpdateToDoVM()
            {
                Id = toDo.Id,
                Title = toDo.Title,
                Deadline = toDo.Deadline,
                Description = toDo.Description,
                IsCompleted = toDo.IsCompleted,

            };

            return View(toDoVM);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateToDoVM vm)
        {

            var toDo = await _context.ToDos.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (toDo == null)
            {
                return NotFound();
            }

            if (vm.Deadline < DateTime.Now.AddDays(-1))
            {
                ModelState.AddModelError("Deadline", "The deadline cannot be in the past");
                return View(vm);
            }

            toDo.Description = vm.Description;
            toDo.Deadline = vm.Deadline;
            toDo.IsCompleted = vm.IsCompleted;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ToDos");
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var toDo = await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            var detailToDoVM = new DetailToDoVM()
            {
                Id = id,
                Title=toDo.Title,
                Deadline=toDo.Deadline,
                Description=toDo.Description,
                CreatedDate=DateTime.Now,
                IsCompleted=toDo.IsCompleted,
            };

            return View(detailToDoVM);

        }
    }
}
