using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context,
                             UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async Task InitializeAsync()
        {
            if (! await _roleManager.RoleExistsAsync(WebsiteRole.WebsiteAdmin)) 
            {
                await _roleManager.CreateAsync(new IdentityRole(WebsiteRole.WebsiteAdmin));
                await _roleManager.CreateAsync(new IdentityRole(WebsiteRole.WebsiteUser));
            }

            await _userManager.CreateAsync(new ApplicationUser
            {
                Id= Guid.NewGuid().ToString(),
                FirstName = "Orkhan",
                LastName = "Mustafayev",
                Email = "orxanm385@gmail.com",
                UserName = "Orkhan1999"
            },"Orxan1999.");

            var appUser = await _userManager.Users.FirstOrDefaultAsync(u=>u.Email== "orxanm385@gmail.com");
            if (appUser != null)
            {
                await _userManager.AddToRoleAsync(appUser, WebsiteRole.WebsiteAdmin);
            }
            
        }

    
    }
}
