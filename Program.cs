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

app.MapGet("/todo", todoHandlers.GetAllToDos);
app.MapGet("/todo/{id}", todoHandlers.GetToDo);
app.MapPost("/todo", todoHandlers.AddToDo);
app.MapPut("/todo/{id}", todoHandlers.ReplaceTodo);
app.MapDelete("/todo/{id}", todoHandlers.DeleteToDo);

FileHandlers fileHandlers = new();

app.MapGet("/file", fileHandlers.DownloadFile);
app.MapGet("/bytes", fileHandlers.DownloadBytes);
app.MapGet("/stream", fileHandlers.DownloadStream);

app.Run();
