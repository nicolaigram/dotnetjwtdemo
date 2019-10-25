using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoapi.Data;
using todoapi.Models;

namespace todoapi.Controllers
{
    [Route("api/todolists")]    
    [ApiController]
    public class TodoListController: ControllerBase
    {

        TodoListContext _todoContext;

        public TodoListController(TodoListContext todoContext)
        {
            _todoContext = todoContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoList>> Get()
        {
            return _todoContext
                .TodoLists
                .Include(todolist => todolist.Todos)
                .ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<TodoList> Get(int id)
        {
            var todolistInDb = _todoContext
                .TodoLists
                .Include(todolist => todolist.Todos)
                .FirstOrDefault(todolist => todolist.Id == id);

            if (todolistInDb == null) return NotFound();

            return todolistInDb;
        }
    }
}
