using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class ForgotPasswordVM
    {
        public string UserId { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
