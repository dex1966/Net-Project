using Microsoft.Data.Sqlite;
using ChatP2P.Core.Models;

namespace ChatP2P.Data.Repositories
{
    public class MessageRepository
    {
        private readonly AppDbContext _dbContext;

        public MessageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(ChatMessage message)
        {
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Messages
                (Id, Type, SenderId, ReceiverId, GroupId, Content, ReplyToId, ForwardedFromId, Timestamp, IsRead)
                VALUES
                ($id, $type, $sender, $receiver, $group, $content, $replyTo, $forwardedFrom, $timestamp, $isRead);
            ";
            command.Parameters.AddWithValue("$id", message.Id);
            command.Parameters.AddWithValue("$type", message.Type.ToString());
            command.Parameters.AddWithValue("$sender", message.SenderId);
            command.Parameters.AddWithValue("$receiver", (object?)message.ReceiverId ?? DBNull.Value);
            command.Parameters.AddWithValue("$group", (object?)message.GroupId ?? DBNull.Value);
            command.Parameters.AddWithValue("$content", message.Content);
            command.Parameters.AddWithValue("$replyTo", (object?)message.ReplyToId ?? DBNull.Value);
            command.Parameters.AddWithValue("$forwardedFrom", (object?)message.ForwardedFromId ?? DBNull.Value);
            command.Parameters.AddWithValue("$timestamp", message.Timestamp.ToString("O"));
            command.Parameters.AddWithValue("$isRead", message.IsRead ? 1 : 0);
            command.ExecuteNonQuery();
        }

        // Lấy lịch sử chat 1-1 giữa 2 peer, phân trang (mới nhất trước)
        public List<ChatMessage> GetDirectHistory(string peerAId, string peerBId, int page, int pageSize = 20)
        {
            var result = new List<ChatMessage>();
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Type, SenderId, ReceiverId, GroupId, Content, ReplyToId, ForwardedFromId, Timestamp, IsRead
                FROM Messages
                WHERE (SenderId = $a AND ReceiverId = $b) OR (SenderId = $b AND ReceiverId = $a)
                ORDER BY Timestamp DESC
                LIMIT $limit OFFSET $offset;
            ";
            command.Parameters.AddWithValue("$a", peerAId);
            command.Parameters.AddWithValue("$b", peerBId);
            command.Parameters.AddWithValue("$limit", pageSize);
            command.Parameters.AddWithValue("$offset", page * pageSize);

            using var reader = command.ExecuteReader();
            while (reader.Read())
                result.Add(ReadMessage(reader));

            return result;
        }

        // Lấy lịch sử chat nhóm, phân trang
        public List<ChatMessage> GetGroupHistory(string groupId, int page, int pageSize = 20)
        {
            var result = new List<ChatMessage>();
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Type, SenderId, ReceiverId, GroupId, Content, ReplyToId, ForwardedFromId, Timestamp, IsRead
                FROM Messages
                WHERE GroupId = $groupId
                ORDER BY Timestamp DESC
                LIMIT $limit OFFSET $offset;
            ";
            command.Parameters.AddWithValue("$groupId", groupId);
            command.Parameters.AddWithValue("$limit", pageSize);
            command.Parameters.AddWithValue("$offset", page * pageSize);

            using var reader = command.ExecuteReader();
            while (reader.Read())
                result.Add(ReadMessage(reader));

            return result;
        }

        private static ChatMessage ReadMessage(SqliteDataReader reader)
        {
            return new ChatMessage
            {
                Id = reader.GetString(0),
                Type = Enum.Parse<MessageType>(reader.GetString(1)),
                SenderId = reader.GetString(2),
                ReceiverId = reader.IsDBNull(3) ? null : reader.GetString(3),
                GroupId = reader.IsDBNull(4) ? null : reader.GetString(4),
                Content = reader.GetString(5),
                ReplyToId = reader.IsDBNull(6) ? null : reader.GetString(6),
                ForwardedFromId = reader.IsDBNull(7) ? null : reader.GetString(7),
                Timestamp = DateTime.Parse(reader.GetString(8)),
                IsRead = reader.GetInt32(9) == 1
            };
        }
    }
}