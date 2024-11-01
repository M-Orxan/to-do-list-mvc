using Microsoft.AspNetCore.Identity;

namespace ToDoList.Models
{
    public class ApplicationUser:IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterDate { get; set; }
        public int ConfirmCode { get; set; }

        public List<ToDo> ToDos { get; set; }
    }
}
