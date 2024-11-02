using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NuGet.Common;
using System.Net.Mail;
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


            return RedirectToAction("Index", "Dashboard");
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
            Random random = new Random();
            int code = random.Next(100000, 1000000);

            var checkByEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (checkByEmail != null)
            {
                ModelState.AddModelError("Email", "This email aready exists");
                return View(vm);
            }


            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                UserName = vm.Username,
                RegisterDate = DateTime.Now,
                ConfirmCode = code

            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, WebsiteRole.WebsiteUser);

                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Admin", "orxanm385@gmail.com"));
                message.To.Add(new MailboxAddress("User", user.Email));

                message.Subject = "Email verification for To Do List";
                var bodyBuiler = new BodyBuilder();
                bodyBuiler.TextBody = "Your verification code: " + code;
                message.Body = bodyBuiler.ToMessageBody();

                MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("orxanm385@gmail.com", "bujg jlxb smmb xfdq");
                client.Send(message);
                client.Disconnect(true);

                TempData["UserId"] = user.Id;

                return RedirectToAction("ConfirmEmail", "Accounts");
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


        public async Task<IActionResult> ConfirmEmail()
        {

            var userId = TempData["UserId"];
            TempData["UserId"] = userId;
            ViewBag.UserId = userId;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailVM vm)
        {

            var userId = TempData["UserId"];
            TempData["UserId"] = userId;
            ViewBag.UserId = userId;
            var user = await _userManager.FindByIdAsync(vm.UserId);

            if (vm.ConfirmCode == null)
            {
                ModelState.AddModelError("ConfirmCode", "Code field is required");
                return View();
            }

            if (user.ConfirmCode == vm.ConfirmCode)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                await _signInManager.SignInAsync(user, true);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                await _userManager.DeleteAsync(user);
                TempData["WarningMessage"] = "Unable to confirm email. Try again";

                return RedirectToAction("Register");

            }





        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "Pages");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM vm)
        {
            if (vm.Email == null)
            {
                ModelState.AddModelError("Email", "Email field is required");
                return View();
            }
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "There's no account with this email");
                return View(vm);
            }
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetTokenLink = Url.Action("ResetPassword", "Accounts", new
            {
                userId = user.Id,
                token = passwordResetToken
            }, HttpContext.Request.Scheme);

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Admin", "orxanm385@gmail.com"));
            message.To.Add(new MailboxAddress("User", vm.Email));

            message.Subject = "Password Reset";
            var bodyBuiler = new BodyBuilder();
            bodyBuiler.TextBody = passwordResetTokenLink;
            message.Body = bodyBuiler.ToMessageBody();


            MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("orxanm385@gmail.com", "bujg jlxb smmb xfdq");
            client.Send(message);
            client.Disconnect(true);
            TempData["SuccessMessage"] = "Password reset link has been sent to your email.";
            return View();
        }

        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            TempData["UserId"] = userId;
            TempData["Token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userId = TempData["UserId"];
            var token = TempData["Token"];

            var user = await _userManager.FindByIdAsync(userId.ToString());

            var result = await _userManager.ResetPasswordAsync(user, token.ToString(), vm.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password has been reset succesfully";
                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }


        }
    }
}
