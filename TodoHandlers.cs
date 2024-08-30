
namespace MinimalAPIs;

public class TodoHandlers
{
    private readonly List<Todo> _todos =
    [
        new Todo
        {
            TodoId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Title = "Learn ASP.NET Core",
            Status = "completed", Categories = new List<string> { "learning", "programming" }
        },
        new Todo
        {
            TodoId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb7"), Title = "Build a web application",
            Status = "pending", Categories = new List<string> { "front end", "back end" }
        },
        new Todo
        {
            TodoId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb8"), Title = "Write documentation",
            Status = "pending", Categories = new List<string> { "work", "writing" }
        }
    ];
    
    public IResult GetToDo(string id, string? humanReadableTitle)
    {
        if (id == "all")
        {
            return Results.Ok(TodoItem.All.Values);
        }

        if (!string.IsNullOrEmpty(humanReadableTitle))
        {
            Console.WriteLine(humanReadableTitle);
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
    
    public IResult GetStatus(string status) 
    {
        var filteredTodos = status.ToLower() switch
        {
            "completed" => _todos.Where(t => t.Status == "completed"),
            "pending" => _todos.Where(t => t.Status == "pending"),
            _ => _todos
        };
    
        return Results.Ok(filteredTodos);
    }
    
    public IResult GetCategories(string? categories) 
    {
        if (string.IsNullOrEmpty(categories))
        {
            return Results.Ok(_todos);
        }

        var categoryList = categories.Split('/').Select(c => c.ToLower()).ToList();
        var filteredToDos = _todos.Where(todo => todo.Categories.Any(cat => categoryList.Contains(cat.ToLower()))).ToList();

        return Results.Ok(filteredToDos);
    }
}