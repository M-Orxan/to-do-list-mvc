using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class UpdateToDoVM
    {

        public int Id { get; set; }
        
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }

       
    }
}
