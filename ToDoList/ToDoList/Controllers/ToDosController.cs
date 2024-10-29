using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    public class ToDosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
