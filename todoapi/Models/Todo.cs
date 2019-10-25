using System;
namespace todoapi.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int TodoListId { get; set; }
        public User AssignedUsers { get; set; }
    }
}
