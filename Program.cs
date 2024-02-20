
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace BU2Chat;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddSingleton<ChatService, ChatService>();

        var app = builder.Build();

        app.MapControllers();
        app.UseHttpsRedirection();

        app.Run();
    }
}

public class Chat
{
    public string Name { get; set; }
    public string Message { get; set; }


    public Chat(string name, string message)
    {
        this.Name = name;
        this.Message = message;
    }
}

public class CreateChatDto
{
    public string Name { get; set; }
    public string Message { get; set; }

    public CreateChatDto(string name, string message)
    {
        this.Name = name;
        this.Message = message;
    }
}

[ApiController]
[Route("api")]
public class ChatController : ControllerBase
{
    private ChatService chatService;

    public ChatController(ChatService chatService)
    {
        this.chatService = chatService;
    }

    [HttpPost("chat")]
    public IActionResult CreateTodo([FromBody] CreateChatDto dto)
    {
        try
        {
            Chat chat = chatService.CreateChat(dto.Name, dto.Message);
            return Ok(chat);
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
    }

    [HttpGet("chats")]
    public List<Chat> GetAllChats()
    {
        return chatService.GetAllChats();
    }

}

public class ChatService
{
    private List<Chat> chats = new List<Chat>();

    public Chat CreateChat(string name, string message)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message cannot be null or whitespace");
        }

        Chat chat = new Chat(name, message);
        chats.Add(chat);
        return chat;
    }

    public List<Chat> GetAllChats()
    {
        return chats;
    }
}


