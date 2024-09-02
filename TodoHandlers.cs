namespace MinimalAPIs;

public class TodoHandlers
{
    public IResult GetToDo(string id, string? humanReadableTitle)
    {
        if (id == "all")
        {
            return Results.Ok(Todo.All.Values);
        }

        if (!string.IsNullOrEmpty(humanReadableTitle))
        {
            Console.WriteLine(humanReadableTitle);
        }
        
        return Todo.All.TryGetValue(id, out var todo)
            ? Results.Ok(todo) 
            : Results.Problem(detail: $"Todo item with ID {id} was not found", statusCode: 404, title: "Not Found", instance: $"/todo/{id}");
    }
    
    public IResult AddToDo(Todo todo)
    {
        if (String.IsNullOrWhiteSpace(todo.Title))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Title", new[] { "Title is required." } }
            });
        }

        Guid todoId = Guid.NewGuid();
        string id = todoId.ToString();
        var newTodo = todo with { Id = id, TodoId = todoId, Title = todo.Title, Status = todo.Status, Description = todo.Description };
        return Todo.All.TryAdd(id, newTodo)
            ? Results.Created($"/todo/{id}", newTodo)
            : Results.BadRequest(new { id = "A Todo item with this id already exists" });
    }
    
    public IResult ReplaceTodo(string id, Todo todo)
    {
        if (Todo.All.ContainsKey(id))
        {
            Todo.All[id] = todo;
            return Results.NoContent();
        }

        return Results.NotFound();
    }
    
    public IResult DeleteToDo(string id)
    {
        return Todo.All.TryRemove(id, out _) ? Results.NoContent() : Results.NotFound();
    }
    
    public IResult GetStatus(string status) 
    {
        var filteredTodos = status.ToLower() switch
        {
            "completed" => Todo.All.Where(t => t.Value.Status == "completed"),
            "pending" => Todo.All.Where(t => t.Value.Status == "pending"),
            _ => Todo.All
        };
    
        return Results.Ok(filteredTodos);
    }
    
    public IResult GetCategories(string? categories) 
    {
        if (string.IsNullOrEmpty(categories))
        {
            return Results.Ok(Todo.All);
        }
        
        var categoryList = categories.Split('/').Select(c => c.ToLower()).ToList();

        var filteredToDos =
            Todo.All.Where(todo =>
                    todo.Value.Categories != null &&
                    todo.Value.Categories.Any(cat => categoryList.Contains(cat.ToLower())))
                .ToList();

        return !filteredToDos.Any() ? Results.NoContent() : Results.Ok(filteredToDos);
    }

    public IResult AddMeTest()
    {
        var addThisTodo = new Todo(Guid.NewGuid(), "100", "Add Me Todo Test")
        {
            Description = "Add Me",
            Status = "pending",
            Categories = new List<string>(["Add Me", "Add Me", "Add Me", "Add Me"])
        };

        return AddToDo(addThisTodo);
    }
}