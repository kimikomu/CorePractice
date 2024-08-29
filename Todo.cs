namespace MinimalAPIs;

public class Todo
{
    public Guid TodoId { get; set; }
    
    public string Title { get; set; }
    
    public string Status { get; set; }
    
    public List<string> Categories { get; set; }
}