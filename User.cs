using System.Collections.Concurrent;

namespace MinimalAPIs;

public record User(int UserId, string Name, List<Todo>? Todos)
{
    public int UserId { get; set; } = UserId;

    public string Name { get; set; } = Name;

    public List<Todo>? Todos { get; set; } = Todos;
    
    public static readonly ConcurrentDictionary<int, User> All = new();
    
    static User()
    {
        All.TryAdd(1, new User(1, "Mike", [
            new Todo(Guid.NewGuid().ToString(), "Buy groceries"), 
            new Todo(Guid.NewGuid().ToString(), "Art")
        ]));
        All.TryAdd(2, new User(2, "Max", [
            new Todo(Guid.NewGuid().ToString(), "What the Heck"), 
            new Todo(Guid.NewGuid().ToString(), "Vacuum")
        ]));
    }
}

