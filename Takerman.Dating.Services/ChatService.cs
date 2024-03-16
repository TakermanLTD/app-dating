using Microsoft.EntityFrameworkCore;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class ChatService(DefaultContext _context) : IChatService
    {
        public async Task<IEnumerable<ChatMessageDto>> GetChatMessagesAsync(int id, int userId)
        {
            return await _context.ChatMessages.Where(x => (x.UserId == id && x.ToUserId == userId) || (x.UserId == userId && x.ToUserId == id))
                .Select(x => new ChatMessageDto()
                {
                    UserId = x.UserId,
                    ToUserId = x.ToUserId,
                    Message = x.Message,
                    SentOn = x.SentOn
                }).OrderByDescending(x => x.SentOn).ToListAsync();
        }

        public async Task SendChatMessageAsync(ChatMessageDto message)
        {
            await _context.ChatMessages.AddAsync(new ChatMessage()
            {
                UserId = message.UserId,
                SentOn = DateTime.UtcNow,
                ToUserId = message.ToUserId,
                Message = message.Message
            });
            await _context.SaveChangesAsync();
        }
    }
}