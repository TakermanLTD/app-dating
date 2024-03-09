namespace Takerman.Dating.Models.DTOs
{
    public class ChatMessageDto
    {
        public int UserId { get; set; }

        public int ToUserId { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime SentOn { get; set; } = DateTime.UtcNow;
    }
}