using goonline.Data;
using goonline.Models;
using goonline.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace goonline.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        public readonly AppDbContext _context;

        public TodoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Todo>> GetAllTodos()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo?> GetTodoById(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task AddTodo(Todo todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateTodo(Todo todo)
        {
            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Todo>> GetIncomingTodos(DateTime startDate, DateTime endDate)
        {
            return await _context.Todos
                .Where(t => t.expiryDate >= startDate && t.expiryDate <= endDate && !t.isDone)
                .ToListAsync();
        }
    }
}
