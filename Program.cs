using MinimalAPIs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

ForecastHandler forecastHandler = new();

app.MapGet("/weatherforecast", forecastHandler.GetForecast)
    .WithName("GetWeatherForecast")
    .WithOpenApi();


var todos = new List<Todo>
{
    new Todo { TodoId = 1, Title = "Learn ASP.NET Core", Status = "completed" },
    new Todo { TodoId = 2, Title = "Build a web application", Status = "pending" }
};

app.MapGet("/todo/status/{status=all}", (string status) =>
{
    var filteredTodos = status.ToLower() switch
    {
        "completed" => todos.Where(t => t.Status == "completed"),
        "pending" => todos.Where(t => t.Status == "pending"),
        _ => todos
    };

    return Results.Ok(filteredTodos);
});

app.MapGet("/todo/{todoId}/{humanReadableTitle?}", (int todoId, string? humanReadableTitle) =>
{
    var todo = todos.FirstOrDefault(t => t.TodoId == todoId);
    if (todo == null)
    {
        return Results.NotFound(new { message = "Todo not found." });
    }

    if (!string.IsNullOrEmpty(humanReadableTitle))
    {
        Console.WriteLine(humanReadableTitle);
    }
    
    return Results.Ok(todo);
});

TodoHandlers todoHandlers = new();

app.MapGet("/todo/{id=all}", todoHandlers.GetToDo);
app.MapPost("/todo", todoHandlers.AddToDo);
app.MapPut("/todo/{id}", todoHandlers.ReplaceTodo);
app.MapDelete("/todo/{id}", todoHandlers.DeleteToDo);

FileHandlers fileHandlers = new();

app.MapGet("/file", fileHandlers.DownloadFile);
app.MapGet("/bytes", fileHandlers.DownloadBytes);
app.MapGet("/stream", fileHandlers.DownloadStream);

var users = new List<User>
{
    new User { UserId = 1, Name = "Alice", Todos = new List<Todo>
    {
        new Todo { TodoId = 1, Title = "Buy groceries" },
        new Todo { TodoId = 2, Title = "Walk the dog" }
    }},
    new User { UserId = 2, Name = "Bob", Todos = new List<Todo>
    {
        new Todo { TodoId = 3, Title = "Complete assignment" },
        new Todo { TodoId = 4, Title = "Call mom" }
    }}
};

app.MapGet("/users/{userId}/todo", (int userId) => 
{
    if (users.Any(u => u.UserId == userId))
    {
        return Results.Ok(users.Where(u => u.UserId == userId).SelectMany(u => u.Todos));
    }
    return Results.Problem(detail: "User not found", statusCode: 404);
});

app.MapGet("/users/{userId}/todo/{todoId}", (int userId, int todoId) => 
{
    if (!users.Any(u => u.UserId == userId))
    {
        return Results.Problem(detail: "User not found", statusCode: 404);
    }
    
    User thisUser = users.First(u => u.UserId == userId);
    if (!thisUser.Todos.Any(t => t.TodoId == todoId))
    {
        return Results.Problem(detail: "Todo not found", statusCode: 404);
    }
    return Results.Ok(thisUser.Todos.FirstOrDefault(t => t.TodoId == todoId));
});

app.Run();
