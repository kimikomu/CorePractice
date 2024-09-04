using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MinimalAPIs;

public class TodoHandlers
{
    // Get by Route: "/todo/{id}"
    public IResult GetToDo(string? id)
    {
        id ??= "all";
        
        if (string.IsNullOrEmpty(id))
        {
            var todo = Todo.All.FirstOrDefault(t => t.Value.Id == id).Value;
            return todo != null ? Results.Ok(todo) : Results.Problem(detail: $"Todo item with ID {id} was not found", statusCode: 404, title: "Not Found", instance: $"/todo/{id}");
        }

        return Results.Ok(Todo.All);
    }
    
    // Post by Body: "/todo"
    public IResult AddToDo([FromBody]Todo? todo)
    {
        // Add new todo if it doesn't exist
        if (todo == null)
        {
            string id = Guid.NewGuid().ToString();
            var newTodo = new Todo(id, "New Todo");
            
            return Todo.All.TryAdd(id, newTodo)
                ? Results.Created($"/todo", newTodo)
                : Results.BadRequest(new { id = "A Todo item with this id already exists" });
        }
        
        // update todo if it does exist
        if (Todo.All.ContainsKey(todo.Id))
        {
            Todo.All[todo.Id] = todo;
            return Results.Ok(todo);
        }
        
        // todo cannot be updated
        return Results.NotFound("Todo with this Id does not exist.");
    }
    
    public IResult DeleteToDo(string id)
    {
        return Todo.All.TryRemove(id, out _) ? Results.NoContent() : Results.NotFound();
    }
    
    // Get by HttpRequest Query: "/todo"
    public IResult GetStatus(HttpRequest request) 
    {
        var filteredTodos = request.Query["status"].ToString().ToLower() switch
        {
            "completed" => Todo.All.Where(t => t.Value.IsCompleted),
            "pending" => Todo.All.Where(t => !t.Value.IsCompleted),
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
        var addThisTodo = new Todo(Guid.NewGuid().ToString(), "Add Me Todo Test")
        {
            Description = "Add Me",
            Status = "pending",
            Categories = new List<string>(["Add Me", "Add Me", "Add Me", "Add Me"])
        };

        return AddToDo(addThisTodo);
    }
}