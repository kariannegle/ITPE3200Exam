using Microsoft.AspNetCore.Http;

public class PostCreate
{
    public string Content { get; set; }
    public IFormFile Image { get; set; }
}
