using System.ComponentModel.DataAnnotations;
using ToDoList.Models;

namespace ToDoList.ViewModels
{
    public class CreateToDoVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime Deadline { get; set; }
        [Required]
       

        public string ApplicationUserId { get; set; }
    }
}
