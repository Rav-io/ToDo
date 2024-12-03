using goonline.Repositories.Interfaces;
using goonline.Services.Interfaces;
using goonline.Models;
using goonline.DTO;
using goonline.Exceptions;

namespace goonline.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Todo>> GetAllTodos()
        {
            return await _repository.GetAllTodos();
        }

        public async Task<Todo?> GetTodoById(int id)
        {
            var todo = await _repository.GetTodoById(id);
            if (todo == null)
            {
                throw new EntityNotFoundException($"Todo with ID {id} does not exist.");
            }
            return await _repository.GetTodoById(id);
        }

        public async Task<IEnumerable<Todo>> GetIncomingTodosForToday()
        {
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);
            return await _repository.GetIncomingTodos(today, tomorrow);
        }
        public async Task<IEnumerable<Todo>> GetIncomingTodosForNextDay()
        {
            var today = DateTime.Now.Date;
            return await _repository.GetIncomingTodos(today.AddDays(1), today.AddDays(2));
        }
        public async Task<IEnumerable<Todo>> GetIncomingTodosForWeek()
        {
            var today = DateTime.Now.Date;
            var endOfWeek = today.AddDays(7);
            return await _repository.GetIncomingTodos(today, endOfWeek);
        }

        public async Task AddTodo(TodoRequestDTO todoRequest)
        {
            var todo = new Todo
            {
                title = todoRequest.title,
                description = todoRequest.description,
                expiryDate = todoRequest.expiryDate,
                isDone = false
            };
            await _repository.AddTodo(todo);
        }

        public async Task UpdateTodo(int id, TodoRequestDTO todoRequest)
        {
            var todo = await _repository.GetTodoById(id);
            if (todo == null)
            {
                throw new EntityNotFoundException($"Todo with ID {id} does not exist.");
            }

            todo.title = todoRequest.title;
            todo.description = todoRequest.description;
            todo.expiryDate = todoRequest.expiryDate;

            await _repository.UpdateTodo(todo);
        }

        public async Task DeleteTodo(int id)
        {
            var todo = await _repository.GetTodoById(id);
            if (todo == null)
            {
                throw new EntityNotFoundException($"Todo with ID {id} does not exist.");
            }
            await _repository.DeleteTodo(id);
        }

        public async Task SetPercentComplete(int id, int percentComplete)
        {
            var todo = await _repository.GetTodoById(id);
            if (todo == null)
            {
                throw new EntityNotFoundException($"Todo with ID {id} does not exist.");
            }
            todo.percentComplete = percentComplete;
            await _repository.UpdateTodo(todo);
        }
        public async Task MarkTodoAsDone(int id)
        {
            var todo = await _repository.GetTodoById(id);
            if (todo == null)
            {
                throw new EntityNotFoundException($"Todo with ID {id} does not exist.");
            }
            todo.isDone = true;
            await _repository.UpdateTodo(todo);
        }
    }
}
