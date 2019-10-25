using System;
using System.Collections.Generic;

namespace todoapi.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Todo> Todos { get; set; }

        public TodoList()
        {
            Todos = new List<Todo>();
        }
    }
}
