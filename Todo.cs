using System.Collections.Concurrent;

namespace MinimalAPIs;

public record Todo(string Id, string Title)
{
    public string? Status { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public string? Description { get; set; }

    public List<string>? Categories { get; set; }
    
    public static readonly ConcurrentDictionary<string, Todo> All = new();
    
    static Todo()
    {
        All.TryAdd("1", new Todo(Id:"3fa85f64-5717-4562-b3fc-2c963f66afb1", Title:"Learn Minimal APIs"));
        All.TryAdd("2", new Todo(Id:"3fa85f64-5717-4562-b3fc-2c963f66afb2", Title:"Build a sample project"));
        All.TryAdd("3", new Todo(Id:"3fa85f64-5717-4562-b3fc-2c963f66afb3", Title:"Create Documentation"));
    }
}
