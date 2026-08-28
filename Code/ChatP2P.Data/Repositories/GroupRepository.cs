using Microsoft.Data.Sqlite;
using ChatP2P.Core.Models;

namespace ChatP2P.Data.Repositories
{
    public class GroupRepository
    {
        private readonly AppDbContext _dbContext;

        public GroupRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(GroupChat group)
        {
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Groups (Id, Name, AvatarPath, CreatedAt)
                VALUES ($id, $name, $avatar, $createdAt);
            ";
            command.Parameters.AddWithValue("$id", group.Id);
            command.Parameters.AddWithValue("$name", group.Name);
            command.Parameters.AddWithValue("$avatar", (object?)group.AvatarPath ?? DBNull.Value);
            command.Parameters.AddWithValue("$createdAt", group.CreatedAt.ToString("O"));
            command.ExecuteNonQuery();

            foreach (var memberId in group.MemberPeerIds)
                AddMember(group.Id, memberId);
        }

        public void AddMember(string groupId, string peerId)
        {
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT OR IGNORE INTO GroupMembers (GroupId, PeerId)
                VALUES ($groupId, $peerId);
            ";
            command.Parameters.AddWithValue("$groupId", groupId);
            command.Parameters.AddWithValue("$peerId", peerId);
            command.ExecuteNonQuery();
        }

        public List<GroupChat> GetAll()
        {
            var groups = new Dictionary<string, GroupChat>();
            using var connection = _dbContext.CreateConnection();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, AvatarPath, CreatedAt FROM Groups;";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var group = new GroupChat
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        AvatarPath = reader.IsDBNull(2) ? null : reader.GetString(2),
                        CreatedAt = DateTime.Parse(reader.GetString(3))
                    };
                    groups[group.Id] = group;
                }
            }

            var memberCommand = connection.CreateCommand();
            memberCommand.CommandText = "SELECT GroupId, PeerId FROM GroupMembers;";
            using (var reader = memberCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    var groupId = reader.GetString(0);
                    var peerId = reader.GetString(1);
                    if (groups.TryGetValue(groupId, out var group))
                        group.MemberPeerIds.Add(peerId);
                }
            }

            return groups.Values.ToList();
        }

        public void Delete(string groupId)
        {
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Groups WHERE Id = $id; DELETE FROM GroupMembers WHERE GroupId = $id;";
            command.Parameters.AddWithValue("$id", groupId);
            command.ExecuteNonQuery();
        }
    }
}