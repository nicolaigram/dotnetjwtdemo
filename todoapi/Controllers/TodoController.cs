using System;
using Microsoft.AspNetCore.Mvc;
using todoapi.Data;
using todoapi.Models;
using System.Linq;


namespace todoapi.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController: ControllerBase
    {
        TodoListContext _todoListContext;

        public TodoController(TodoListContext todoListContext)
        {
            _todoListContext = todoListContext;
        }

        [HttpPost]
        public ActionResult<Todo> Create([FromBody] Todo todo)
        {
            var todolistToAppend = _todoListContext.TodoLists.FirstOrDefault(todolist => todolist.Id == todo.TodoListId);
            if (todolistToAppend == null) return NotFound("Invalid TodoListId");

            var newTodo = new Todo() { Title = todo.Title, TodoListId = todo.TodoListId, Completed = false };

            var addedTodo = _todoListContext.Todos.Add(newTodo);
            _todoListContext.SaveChanges();

            return addedTodo.Entity;
        }
        
    }
}
