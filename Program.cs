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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

var todos = new Dictionary<string, TodoItem>
{
    { "1", new TodoItem("1", "Sample Task 1", false) },
    { "2", new TodoItem("2", "Sample Task 2", true) }
};

// GET
// 1. An endpoint to get all TODO items at the route /todo
app.MapGet("/todo", () => todos.Values);
// 2. An endpoint to get a TODO item by its ID at the route /todo/{id}
app.MapGet("/todo/{id}", (string id) => todos.GetValueOrDefault(id));

// POST
// An endpoint to add a new todo item to todos
app.MapPost("/todo", (TodoItem todo) =>
{
    string id = Guid.NewGuid().ToString();
    var newTodo = new TodoItem(id, todo.Description, todo.IsCompleted);
    todos.Add(id, newTodo);
    return Results.Created($"/todo/{id}", id);
});

// PUT
// An endpoint to update a todo by id
app.MapPut("/todo/{id}", (TodoItem todo) =>
{
    todos[todo.Id] = todo;
    Results.Accepted("/todo/{id}", todo.Id);
});

// DELETE
// An endpoint to remove a todo by id
app.MapDelete("/todo/{id}", (string id) =>
{
    todos.Remove(id);
    Results.Accepted("/todo");
});

app.Run();

public record TodoItem(string Id, string Description, bool IsCompleted);
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}