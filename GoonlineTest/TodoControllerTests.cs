using goonline.Controllers;
using goonline.Services.Interfaces;
using FakeItEasy;
using goonline.Models;
using Microsoft.AspNetCore.Mvc;
using goonline.DTO;
using Microsoft.Extensions.Logging;

namespace GoonlineTest
{
    public class TodoControllerTests
    {
        private readonly TodoController _todoController;
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoController> _logger;

        public TodoControllerTests()
        {
            _todoService = A.Fake<ITodoService>();
            _logger = A.Fake<ILogger<TodoController>>();
            _todoController = new TodoController(_todoService, _logger);
        }

        //Test to ensure the controller's GetAllTodos method returns all todos successfully
        [Fact]
        public async Task GetAllTodos_ReturnsTodos()
        {
            // Arrange
            var sampleTodo1 = new Todo
            {
                id = 1,
                title = "Test task 1",
                description = "This is a description for task 1",
                expiryDate = DateTime.Now,
                percentComplete = 0,
                isDone = false,
            };
            var sampleTodo2 = new Todo
            {
                id = 2,
                title = "Test task 2",
                description = "This is a description for task 2",
                expiryDate = DateTime.Now.AddDays(1),
                percentComplete = 50,
                isDone = false,
            };
            var todos = new List<Todo> { sampleTodo1, sampleTodo2 };

            A.CallTo(() => _todoService.GetAllTodos()).Returns(todos);

            // Act
            var result = await _todoController.GetAllTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTodos = Assert.IsType<List<Todo>>(okResult.Value);
            Assert.Equal(2, returnedTodos.Count);
        }

        //Test to ensure the controller's GetTodoById method returns the correct todo
        [Fact]
        public async Task GetTodoById_ReturnsTodo()
        {
            // Arrange
            var sampleTodo = new Todo
            {
                id = 9,
                title = "Test task 9",
                description = "This is a description for task 9",
                expiryDate = DateTime.Now,
                percentComplete = 0,
                isDone = false,
            };
            int selectedId = 9;
            var todo = new Todo();
            A.CallTo(() => _todoService.GetTodoById(selectedId)).Returns(sampleTodo);

            //Act
            var result = await _todoController.GetTodoById(selectedId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTodo = Assert.IsType<Todo>(okResult.Value);
            Assert.Equal(selectedId, returnedTodo.id);
        }

        //Test to ensure that when a todo is created, the controller returns a 201 status
        [Fact]
        public async Task CreateTodo_ReturnsCreated_WithTodo()
        {
            // Arrange
            var todoRequest = new TodoRequestDTO { title = "Test Todo", description = "Test Description", expiryDate = DateTime.Now };
            A.CallTo(() => _todoService.AddTodo(todoRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _todoController.CreateTodo(todoRequest);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }
    }
}
