using Microsoft.AspNetCore.Mvc;
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

TodoHandlers todoHandlers = new();

app.MapGet("/todo/{id=all}/{humanReadableTitle?}", todoHandlers.GetToDo);
app.MapPost("/todo", todoHandlers.AddToDo);
app.MapPut("/todo/{id:guid}", todoHandlers.ReplaceTodo);
app.MapDelete("/todo/{id:guid}", todoHandlers.DeleteToDo);
app.MapGet("/todo/status/{status=all}", todoHandlers.GetStatus);
app.MapGet("/todo/status", ([FromQuery] string status) => Todo.All.Where(t => t.Value.Status != null && t.Value.Status.Equals(status, StringComparison.Ordinal)));
app.MapGet("/todo/categories/{**categories}", todoHandlers.GetCategories);

FileHandlers fileHandlers = new();

app.MapGet("/file", fileHandlers.DownloadFile);
app.MapGet("/bytes", fileHandlers.DownloadBytes);
app.MapGet("/stream", fileHandlers.DownloadStream);

UserHandlers userHandlers = new();

app.MapGet("/users/{userId}/todo", userHandlers.GetUser);
app.MapGet("/users/{userId}/todo/{todoId}", userHandlers.GetUserTodo);

app.Run();
