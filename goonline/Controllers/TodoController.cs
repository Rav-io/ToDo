using goonline.DTO;
using goonline.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using goonline.Exceptions;
using Microsoft.Extensions.Logging;

namespace goonline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;
        private readonly ILogger<TodoController> _logger;
        public TodoController(ITodoService service, ILogger<TodoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // Retrieve all todos
        // Example: GET /api/todo
        //  - 200 OK: Returns the list of todos.
        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            _logger.LogInformation("Fetching all todos.");
            var todos = await _service.GetAllTodos();

            _logger.LogInformation("Found {Count} todos.", todos.Count());
            return Ok(todos);
        }
        // Retrieve a todo by its ID
        // Example: GET /api/todo/id/1
        //  - 200 OK: Returns the todo with the specified ID.
        //  - 404 Not Found: If the todo with the specified ID is not found.
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            _logger.LogInformation("Fetching todo with ID {Id}.", id);
            try
            {
                var todo = await _service.GetTodoById(id);
                _logger.LogInformation("Todo with ID {Id} found successfully.", id);
                return Ok(todo);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning("Todo with ID {Id} not found. Exception: {Exception}", id, ex.Message);
                return NotFound(new { error = ex.Message });
            }
        }
        // Retrieve todos that are due today
        // Example: GET /api/todo/today
        //  - 200 OK: Returns the list of todos that are due today.
        [HttpGet("today")]
        public async Task<IActionResult> GetIncomingTodosForToday()
        {
            _logger.LogInformation("Fetching todos due today.");
            var todos = await _service.GetIncomingTodosForToday();

            _logger.LogInformation("Found {Count} todos due today.", todos.Count());
            return Ok(todos);
        }

        // Retrieve todos that are due the next day
        // Example: GET /api/todo/nextDay
        //  - 200 OK: Returns the list of todos that are due the next day.
        [HttpGet("nextDay")]
        public async Task<IActionResult> GetIncomingTodosForNextDay()
        {
            _logger.LogInformation("Fetching todos due next day.");
            var todos = await _service.GetIncomingTodosForNextDay();

            _logger.LogInformation("Found {Count} todos due next day.", todos.Count());
            return Ok(todos);
        }

        // Retrieve todos that are due in the upcoming week
        // Example: GET /api/todo/week
        //  - 200 OK: Returns the list of todos that are due in the next week.
        [HttpGet("week")]
        public async Task<IActionResult> GetIncomingTodosForWeek()
        {
            _logger.LogInformation("Fetching todos due this week.");
            var todos = await _service.GetIncomingTodosForWeek();

            _logger.LogInformation("Found {Count} todos due this week.", todos.Count());
            return Ok(todos);
        }

        // Create a new todo
        // Example: POST /api/todo/create
        //  - 201 Created: Returns the created todo object.
        //  - 400 Bad Request: If the model validation fails.
        [HttpPost("create")]
        public async Task<IActionResult> CreateTodo([FromBody] TodoRequestDTO todo)
        {
            _logger.LogInformation("Creating a new todo: {Title}.", todo.title);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed for the new todo.");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(todo.title))
            {
                _logger.LogWarning("Title cannot be empty or whitespace.");
                return BadRequest(new { error = "Title cannot be empty or whitespace." });
            }

            await _service.AddTodo(todo);
            _logger.LogInformation("Todo created successfully with title: {Title}.", todo.title);
            return StatusCode(201, todo);
        }

        // Update an existing todo
        // Example: PUT /api/todo/update/1
        //  - 200 OK: Returns a success message.
        //  - 404 Not Found: If the todo with the specified ID is not found.
        //  - 400 Bad Request: If the model validation fails.
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoRequestDTO todo)
        {
            _logger.LogInformation("Updating todo with ID {Id}.", id);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed for updating todo with ID {Id}.", id);
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(todo.title))
            {
                _logger.LogWarning("Title cannot be empty or whitespace for updating todo with ID {Id}.", id);
                return BadRequest(new { error = "Title cannot be empty or whitespace." });
            }

            try
            {
                await _service.UpdateTodo(id, todo);
                _logger.LogInformation("Todo with ID {Id} updated successfully.", id);
                return Ok($"Todo with ID {id} has been updated successfully.");
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning("Todo with ID {Id} not found. Exception: {Exception}", id, ex.Message);
                return NotFound(new { error = ex.Message });
            }
        }

        // Delete a todo
        // Example: DELETE /api/todo/delete/1
        //  - 200 OK: Returns a success message confirming the todo was deleted.
        //  - 404 Not Found: If the todo with the specified ID is not found.
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            _logger.LogInformation("Deleting todo with ID {Id}.", id);
            try
            {
                await _service.DeleteTodo(id);
                _logger.LogInformation("Todo with ID {Id} deleted successfully.", id);
                return Ok($"Todo with ID {id} has been deleted successfully.");
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning("Todo with ID {Id} not found. Exception: {Exception}", id, ex.Message);
                return NotFound(new { error = ex.Message });
            }

        }

        // Update the "percent complete" of a todo
        // Example: PATCH /api/todo/1/percentComplete/80
        //  - 200 OK: Returns a success message confirming the percent complete has been updated.
        //  - 400 Bad Request: If the percent complete is greater than 100 or validation fails.
        //  - 404 Not Found: If the todo with the specified ID is not found.
        [HttpPatch("{id}/percentComplete/{percentComplete}")]
        public async Task<IActionResult> SetPercentComplete(int id, int percentComplete)
        {
            _logger.LogInformation("Updating percent complete for todo with ID {Id}.", id);
            try
            {
                if (percentComplete > 100)
                {
                    _logger.LogWarning("Percent complete cannot exceed 100 for todo with ID {Id}.", id);
                    return BadRequest("Percent complete cannot exceed 100.");
                }
                await _service.SetPercentComplete(id, percentComplete);

                _logger.LogInformation("Percent complete updated for todo with ID {Id}.", id);
                return Ok("Percent complete has been updated.");
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning("Todo with ID {Id} not found. Exception: {Exception}", id, ex.Message);
                return NotFound(new { error = ex.Message });
            }
        }

        // Mark todo as "done"
        // Example: PATCH /api/todo/1/markAsDone
        //  - 200 OK: Returns a success message confirming the todo was marked as done.
        //  - 404 Not Found: If the todo with the specified ID is not found.
        [HttpPatch("{id}/markAsDone")]
        public async Task<IActionResult> MarkAsDone(int id)
        {
            _logger.LogInformation("Marking todo with ID {Id} as done.", id);
            try
            {
                await _service.MarkTodoAsDone(id);

                _logger.LogInformation("Todo with ID {Id} marked as done.", id);
                return Ok($"Todo with ID {id} has been marked as done.");
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning("Todo with ID {Id} not found. Exception: {Exception}", id, ex.Message);
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
