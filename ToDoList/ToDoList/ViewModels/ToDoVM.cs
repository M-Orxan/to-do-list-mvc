using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class ToDoVM
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        
        public DateTime Deadline { get; set; }

        public bool IsCompleted { get; set; }


        public string ApplicationUserId { get; set; }
    }
}
