using System.Collections.Concurrent;

namespace MinimalAPIs;

public record Todo(Guid TodoId, string Id, string Title)
{
    public string Id = Id;

    public Guid TodoId { get; set; } = TodoId;

    public string Title { get; set; } = Title;

    public string? Status { get; set; }
    
    public string? Description { get; set; }

    public List<string>? Categories { get; set; }
    
    public static readonly ConcurrentDictionary<string, Todo> All = new();
    
    static Todo()
    {
        All.TryAdd("1", new Todo(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb1"), "3fa85f64-5717-4562-b3fc-2c963f66afb1", "Learn Minimal APIs"));
        All.TryAdd("2", new Todo(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb2"), "3fa85f64-5717-4562-b3fc-2c963f66afb2", "Build a sample project"));
        All.TryAdd("3", new Todo(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb3"),"3fa85f64-5717-4562-b3fc-2c963f66afb3", "Write Documentation"));
    }
}
