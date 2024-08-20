using Microsoft.EntityFrameworkCore;

namespace todo;

public class TodoDb:DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options):base(options)
    {
        
    }
    public DbSet<TodoItem> TodoItems { get; set; }
}
