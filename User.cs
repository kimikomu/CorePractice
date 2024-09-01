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
            new Todo(Guid.NewGuid(), "10", "Buy groceries"), 
            new Todo(Guid.NewGuid(), "11", "Art")
        ]));
        All.TryAdd(2, new User(2, "Max", [
            new Todo(Guid.NewGuid(), "12", "What the Heck"), 
            new Todo(Guid.NewGuid(), "13", "Vacuum")
        ]));
    }
}

