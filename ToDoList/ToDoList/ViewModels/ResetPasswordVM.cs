using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage ="Passwords don't match")]
     
        public string ConfirmPassword { get; set; }
    }
}
