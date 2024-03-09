using Takerman.Dating.Models.DTOs;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMessageDto>> GetChatMessagesAsync(int id, int userId);

        Task SendChatMessageAsync(ChatMessageDto message);
    }
}