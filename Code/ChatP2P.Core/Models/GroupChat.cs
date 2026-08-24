using System;
using System.Collections.Generic;

namespace ChatP2P.Core.Models
{
    public class GroupChat
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public List<string> MemberPeerIds { get; set; } = new();
        public string? AvatarPath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public void AddMember(string peerId)
        {
            if (!MemberPeerIds.Contains(peerId))
                MemberPeerIds.Add(peerId);
        }

        public void RemoveMember(string peerId)
        {
            MemberPeerIds.Remove(peerId);
        }
    }
}