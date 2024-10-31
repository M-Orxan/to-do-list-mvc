using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class ContactFormVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
       // public string MessageStatus { get; set; } 
        public DateTime SubmittedAt { get; set; }

    }
}
