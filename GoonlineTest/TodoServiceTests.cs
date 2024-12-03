using goonline.Repositories.Interfaces;
using goonline.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using goonline.DTO;
using goonline.Models;
using goonline.Services.Interfaces;
using goonline.Exceptions;

namespace GoonlineTest
{
    public class TodoServiceTests
    {
        private readonly TodoService _service;
        private readonly ITodoRepository _repository;

        public TodoServiceTests()
        {
            _repository = A.Fake<ITodoRepository>();
            _service  = new TodoService(_repository);
        }

        //Test to ensure a new todo is added successfully
        [Fact]
        public async Task AddTodo_ShouldAddNewTodo()
        {
            // Arrange
            var todoRequest = new TodoRequestDTO
            {
                title = "New Todo",
                description = "Description for new Todo",
                expiryDate = DateTime.Now.AddDays(2)
            };

            // Act
            await _service.AddTodo(todoRequest);

            // Assert
            A.CallTo(() => _repository.AddTodo(A<Todo>.That.Matches(t => t.title == "New Todo"))).MustHaveHappenedOnceExactly();
        }

        //Test to ensure that an existing todo is updated correctly
        [Fact]
        public async Task UpdateTodo_ShouldUpdateTodo()
        {
            //Arrange
            var todoRequest = new TodoRequestDTO
            {
                title = "Updated Todo",
                description = "Updated description",
                expiryDate = DateTime.Now.AddDays(3)
            };

            var existingTodo = new Todo
            {
                id = 1,
                title = "Old Todo",
                description = "Old description",
                expiryDate = DateTime.Now
            };
            A.CallTo(() => _repository.GetTodoById(1)).Returns(existingTodo);

            // Act
            await _service.UpdateTodo(1, todoRequest);

            // Assert
            A.CallTo(() => _repository.UpdateTodo(A<Todo>.That.Matches(t => t.title == "Updated Todo"))).MustHaveHappenedOnceExactly();
        }

        //Test to ensure the percentComplete property is updated correctly
        [Fact]
        public async Task SetPercentComplete_ShouldUpdatePercentComplete()
        {
            // Arrange
            var existingTodo = new Todo
            {
                id = 1,
                title = "Todo 1",
                description = "Description 1",
                expiryDate = DateTime.Now,
                percentComplete = 50
            };
            A.CallTo(() => _repository.GetTodoById(1)).Returns(existingTodo);

            // Act
            await _service.SetPercentComplete(1, 75);

            // Assert
            Assert.Equal(75, existingTodo.percentComplete);
        }

        //Test to ensure the todo is marked as done correctly
        [Fact]
        public async Task MarkTodoAsDone_ShouldMarkAsDone()
        {
            // Arrange
            var existingTodo = new Todo
            {
                id = 1,
                title = "Todo 1",
                description = "Description 1",
                expiryDate = DateTime.Now,
                isDone = false
            };
            A.CallTo(() => _repository.GetTodoById(1)).Returns(existingTodo);

            // Act
            await _service.MarkTodoAsDone(1);

            // Assert
            Assert.True(existingTodo.isDone);
        }
    }
}
