namespace MinimalAPIs;

public class UserHandlers
{
    private List<User> _users = new List<User>
    {
        new User { UserId = 1, Name = "Alice", Todos = new List<Todo>
        {
            new Todo { TodoId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb1"), Title = "Buy groceries" },
            new Todo { TodoId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb2"), Title = "Walk the dog" }
        }},
        new User { UserId = 2, Name = "Bob", Todos = new List<Todo>
        {
            new Todo { TodoId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb3"), Title = "Complete assignment" },
            new Todo { TodoId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb4"), Title = "Call mom" }
        }}
    };


    public IResult GetUser(int userId)
    {
        if (_users.Any(u => u.UserId == userId))
        {
            return Results.Ok(_users.Where(u => u.UserId == userId).SelectMany(u => u.Todos));
        }
        return Results.Problem(detail: "User not found", statusCode: 404);
    }

    public IResult GetUserTodo(int userId, Guid todoId)
    {
        if (_users.All(u => u.UserId != userId))
        {
            return Results.Problem(detail: "User not found", statusCode: 404);
        }
    
        User thisUser = _users.First(u => u.UserId == userId);
        if (thisUser.Todos.All(t => t.TodoId != todoId))
        {
            return Results.Problem(detail: "Todo not found", statusCode: 404);
        }
        return Results.Ok(thisUser.Todos.FirstOrDefault(t => t.TodoId == todoId));
    }
}