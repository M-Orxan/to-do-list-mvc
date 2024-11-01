using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class ConfirmEmailVM
    {

        public string UserId { get; set; }
        [Required]
        public int? ConfirmCode { get; set; }

    }
}
