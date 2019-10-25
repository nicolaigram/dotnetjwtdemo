using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using todoapi.Models;

namespace todoapi.Data
{
    public class TodoListContext: DbContext
    {
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users{ get; set; }

        public TodoListContext(DbContextOptions<TodoListContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoList>().HasData(
                new TodoList { Id = 1, Name = "My list!"}
            );

            modelBuilder.Entity<Todo>().HasData(
                new Todo { Id = 1, Title = "Task 1", Completed = false, TodoListId = 1 }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Nicolai", LastName = "Gram", Username = "ngram", Role = Role.Admin, Password = "123" }
            );
        }
    }
}
