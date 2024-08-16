
namespace MinimalAPIs;

public class TodoHandlers
{
    public IEnumerable<TodoItem> GetAllToDos()
    {
        return TodoItem.All.Values;
    }
    
    public IResult GetToDo(string id)
    {
        return TodoItem.All.TryGetValue(id, out var todo) ? Results.Ok(todo) : Results.NotFound();
    }
    
    public IResult AddToDo(TodoItem todo)
    {
        string id = Guid.NewGuid().ToString();
        var newTodo = todo with { Id = id };
        return TodoItem.All.TryAdd(id, newTodo)
            ? Results.Created($"/todo/{id}", newTodo)
            : Results.BadRequest(new { id = "A Todo item with this id already exists" });
    }
    
    public IResult ReplaceTodo(string id, TodoItem todo)
    {
        if (TodoItem.All.ContainsKey(id))
        {
            TodoItem.All[id] = todo;
            return Results.NoContent();
        }

        return Results.NotFound();
    }
    
    public IResult DeleteToDo(string id)
    {
        return TodoItem.All.TryRemove(id, out _) ? Results.NoContent() : Results.NotFound();
    }
}