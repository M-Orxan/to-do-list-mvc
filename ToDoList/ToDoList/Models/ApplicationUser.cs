using Microsoft.AspNetCore.Identity;

namespace ToDoList.Models
{
    public class ApplicationUser:IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<ToDo> ToDos { get; set; }
    }
}
