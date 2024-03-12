using System.Text.Json.Serialization;

namespace Takerman.Dating.Models.DTOs
{
    public class UploadUserPicturesDto
    {
        public int UserId { get; set; }

        public byte[] Picture { get; set; }
    }
}