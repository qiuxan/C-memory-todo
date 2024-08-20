
using Microsoft.EntityFrameworkCore;

namespace todo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<TodoDb>(options =>
            {
                options.UseInMemoryDatabase("TodoList");
            });

            var app = builder.Build();

            //create crud operations for todo items using MapGet, MapPost, MapPut, MapDelete
            app.MapGet("/todo", async (TodoDb db) =>
            {
                return Results.Ok(await db.TodoItems.ToListAsync());
            });

            app.MapGet("/todo/{id}", async (TodoDb db, int id) =>
            {
                var item = await db.TodoItems.FindAsync(id);
                if (item == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(item);
            });

            app.MapPost("/todo", async (TodoDb db, TodoItem item) =>
            {
                db.TodoItems.Add(item);
                await db.SaveChangesAsync();
                return Results.Created($"/todo/{item.Id}", item);
            });

            app.MapPut("/todo/{id}", async (TodoDb db, int id, TodoItem item) =>
            {
                if (id != item.Id)
                {
                    return Results.BadRequest();
                }
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            app.MapDelete("/todo/{id}", async (TodoDb db, int id) =>
            {
                var item = await db.TodoItems.FindAsync(id);
                if (item == null)
                {
                    return Results.NotFound();
                }
                db.TodoItems.Remove(item);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // Configure the HTTP request pipeline.
       
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
