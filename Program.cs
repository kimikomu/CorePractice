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
    new Todo { TodoId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Title = "Learn ASP.NET Core", Status = "completed" },
    new Todo { TodoId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb7"), Title = "Build a web application", Status = "pending" }
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

app.MapGet("/todo/{todoId:guid}/{humanReadableTitle?}", (Guid todoId, string? humanReadableTitle) =>
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

app.MapGet("/todo/{id=all:guid}", todoHandlers.GetToDo);
app.MapPost("/todo", todoHandlers.AddToDo);
app.MapPut("/todo/{id:guid}", todoHandlers.ReplaceTodo);
app.MapDelete("/todo/{id:guid}", todoHandlers.DeleteToDo);

FileHandlers fileHandlers = new();

app.MapGet("/file", fileHandlers.DownloadFile);
app.MapGet("/bytes", fileHandlers.DownloadBytes);
app.MapGet("/stream", fileHandlers.DownloadStream);

var users = new List<User>
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

app.MapGet("/users/{userId}/todo", (int userId) => 
{
    if (users.Any(u => u.UserId == userId))
    {
        return Results.Ok(users.Where(u => u.UserId == userId).SelectMany(u => u.Todos));
    }
    return Results.Problem(detail: "User not found", statusCode: 404);
});

app.MapGet("/users/{userId}/todo/{todoId:guid}", (int userId, Guid todoId) => 
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
