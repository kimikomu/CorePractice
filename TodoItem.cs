using System.Collections.Concurrent;

namespace MinimalAPIs;

public record TodoItem(string Id, string Description, bool IsComplete)
{
    public static readonly ConcurrentDictionary<string, TodoItem> All = new();

    static TodoItem()
    {
        All.TryAdd("1", new TodoItem("1", "Learn Minimal APIs", false));
        All.TryAdd("2", new TodoItem("2", "Build a sample project", true));
    }
}