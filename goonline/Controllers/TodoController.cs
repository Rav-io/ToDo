using goonline.DTO;
using goonline.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using goonline.Exceptions;

namespace goonline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;
        public TodoController(ITodoService service)
        {
            _service = service;
        }

        // Retrieve all todos
        // Example: GET /api/todo
        //  - 200 OK: Returns the list of todos.
        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            var todos = await _service.GetAllTodos();
            return Ok(todos);
        }
        // Retrieve a todo by its ID
        // Example: GET /api/todo/id/1
        //  - 200 OK: Returns the todo with the specified ID.
        //  - 404 Not Found: If the todo with the specified ID is not found.
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            try
            {
                var todo = await _service.GetTodoById(id);
                return Ok(todo);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        // Retrieve todos that are due today
        // Example: GET /api/todo/today
        //  - 200 OK: Returns the list of todos that are due today.
        [HttpGet("today")]
        public async Task<IActionResult> GetIncomingTodosForToday()
        {
            var todos = await _service.GetIncomingTodosForToday();
            return Ok(todos);
        }

        // Retrieve todos that are due the next day
        // Example: GET /api/todo/nextDay
        //  - 200 OK: Returns the list of todos that are due the next day.
        [HttpGet("nextDay")]
        public async Task<IActionResult> GetIncomingTodosForNextDay()
        {
            var todos = await _service.GetIncomingTodosForNextDay();
            return Ok(todos);
        }

        // Retrieve todos that are due in the upcoming week
        // Example: GET /api/todo/week
        //  - 200 OK: Returns the list of todos that are due in the next week.
        [HttpGet("week")]
        public async Task<IActionResult> GetIncomingTodosForWeek()
        {
            var todos = await _service.GetIncomingTodosForWeek();
            return Ok(todos);
        }

        // Create a new todo
        // Example: POST /api/todo/create
        //  - 201 Created: Returns the created todo object.
        //  - 400 Bad Request: If the model validation fails.
        [HttpPost("create")]
        public async Task<IActionResult> CreateTodo([FromBody] TodoRequestDTO todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddTodo(todo);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateTodo(id, todo);
                return Ok($"Todo with ID {id} has been updated successfully.");
            }
            catch (EntityNotFoundException ex)
            {
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
            try
            {
                await _service.DeleteTodo(id);
                return Ok($"Todo with ID {id} has been deleted successfully.");
            }
            catch (EntityNotFoundException ex)
            {
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
            try
            {
                if (percentComplete > 100)
                {
                    return BadRequest("Percent complete cannot exceed 100.");
                }
                await _service.SetPercentComplete(id, percentComplete);

                return Ok("Percent complete has been updated.");
            }
            catch (EntityNotFoundException ex)
            {
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
            try
            {
                await _service.MarkTodoAsDone(id);
                return Ok($"Todo with ID {id} has been marked as done.");
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
