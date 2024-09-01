namespace MinimalAPIs;

public class UserHandlers
{
    public IResult GetUser(int userId)
    {
        return User.All.TryGetValue(userId, out var user) 
            ? Results.Ok(user) 
            : Results.Problem("User not found", statusCode: 404, instance:$"/user/{userId}");
    }

    public IResult GetUserTodo(int userId, string todoId)
    {
        if (!User.All.TryGetValue(userId, out var user))
        {
            return Results.Problem(detail: $"User {user} not found", statusCode: 404);
        }
        
        if (user.Todos != null)
        {
            var todo = user.Todos.FirstOrDefault(todo => todo.Id == todoId);

            return todo != null
                ? Results.Ok(todo)
                : Results.Problem("User Todo not found", statusCode: 404,
                    instance: $"/user/{userId}/todo/{todoId}");
        }

        return Results.Problem(detail: $"User {user} not found", statusCode: 404);
    }
}
