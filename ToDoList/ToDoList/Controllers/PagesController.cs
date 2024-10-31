using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _logger;
        private readonly IEmailSender _emailSender;

        public PagesController(ILogger<PagesController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }


        public IActionResult Contact()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(ContactFormVM vm)
        {
           
            if (!ModelState.IsValid)
            {
                return View("Contact");
            }
            try
            {
                _emailSender.SendEmailAsync(vm.Name, vm.Email, vm.Subject, vm.Message);
                TempData["MessageStatus"] = "Message sent successfully";
            }
            catch
            {
                TempData["MessageStatus"] = "Message sending failed";
            }
           

            return RedirectToAction("Contact",vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
