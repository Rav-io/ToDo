using goonline.Models;

namespace goonline.Repositories.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllTodos();
        Task<Todo?> GetTodoById(int id);
        Task<IEnumerable<Todo>> GetIncomingTodos(DateTime startDate, DateTime endDate);
        Task AddTodo(Todo todo);
        Task UpdateTodo(Todo todo);
        Task DeleteTodo(int id);
    }
}
