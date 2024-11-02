namespace ToDoList.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int? ConfirmCode { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
