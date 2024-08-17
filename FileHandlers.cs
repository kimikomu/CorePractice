using System.Text;

namespace MinimalAPIs;

public class FileHandlers
{
    public IResult DownloadFile()
    {
        var filePath = "/usercode/FILESYSTEM/MinimalAPIs/todos.txt"; // Ensure this file exists with some content
        var contentType = "text/plain";
        return Results.File(filePath, contentType, "todos.txt");
    }

    public IResult DownloadBytes()
    {
        var todos = "1. Learn ASP.Net Core\n2. Build a web application.";
        var bytes = Encoding.UTF8.GetBytes(todos);
        return Results.Bytes(bytes, "application/octet-stream", "todos.bin");
    }

    public IResult DownloadStream()
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write("1. Learn ASP.Net Core\n2. Build a web application.");
        writer.Flush();
        stream.Position = 0;

        return Results.Stream(stream, "application/octet-stream", "todos.txt");
    }
}
