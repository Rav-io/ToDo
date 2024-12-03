using goonline.DTO;
using goonline.Models;

namespace goonline.Services.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAllTodos();
        Task<Todo?> GetTodoById(int id);
        Task<IEnumerable<Todo>> GetIncomingTodosForToday();
        Task<IEnumerable<Todo>> GetIncomingTodosForNextDay();
        Task<IEnumerable<Todo>> GetIncomingTodosForWeek();
        Task AddTodo(TodoRequestDTO todoRequest);
        Task UpdateTodo(int id, TodoRequestDTO todoRequest);
        Task DeleteTodo(int id);
        Task SetPercentComplete(int id, int percentComplete);
        Task MarkTodoAsDone(int id);
    }
}
