namespace MinimalAPIs;

public class User
{
    public int UserId { get; set; }
    
    public string Name { get; set; }
    
    public List<Todo> Todos { get; set; }
}

