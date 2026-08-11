using System;

namespace ChatP2P.Core.Models
{
    public class Peer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public int Port { get; set; }
        public string? AvatarPath { get; set; }
        public bool IsOnline { get; set; } = false;
        public DateTime LastSeen { get; set; } = DateTime.Now;

        public override string ToString() => $"{Name} ({IpAddress}:{Port})";
    }
}