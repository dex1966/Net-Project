using System;

namespace ChatP2P.Core.Models
{
    public enum MessageType
    {
        Text,
        Image,
        File,
        System
    }

    public class ChatMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public MessageType Type { get; set; } = MessageType.Text;
        public string SenderId { get; set; } = string.Empty;
        public string? ReceiverId { get; set; }     // dùng khi chat 1-1
        public string? GroupId { get; set; }         // dùng khi chat nhóm
        public string Content { get; set; } = string.Empty;
        public string? ReplyToId { get; set; }
        public string? ForwardedFromId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}