
namespace MinimalAPIs;

public class TodoHandlers
{
    public IResult GetToDo(string id)
    {
        if (id == "all")
        {
            return Results.Ok(TodoItem.All.Values);
        }
        
        return TodoItem.All.TryGetValue(id, out var todo) 
            ? Results.Ok(todo) 
            : Results.Problem(detail: $"Todo item with ID {id} was not found", statusCode: 404, title: "Not Found", instance: $"/todo/{id}");
    }
    
    public IResult AddToDo(TodoItem todo)
    {
        if (String.IsNullOrWhiteSpace(todo.Description))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Description", new[] { "Description is required." } }
            });
        }
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