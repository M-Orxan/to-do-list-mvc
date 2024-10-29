using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class LoginVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
       
        public string Password { get; set; }
    }
}
