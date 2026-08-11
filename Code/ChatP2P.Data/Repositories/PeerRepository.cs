using Microsoft.Data.Sqlite;
using ChatP2P.Core.Models;

namespace ChatP2P.Data.Repositories
{
    public class PeerRepository
    {
        private readonly AppDbContext _dbContext;

        public PeerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddOrUpdate(Peer peer)
        {
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Peers (Id, Name, IpAddress, Port, AvatarPath, IsOnline, LastSeen)
                VALUES ($id, $name, $ip, $port, $avatar, $online, $lastSeen)
                ON CONFLICT(Id) DO UPDATE SET
                    Name = $name, IpAddress = $ip, Port = $port,
                    AvatarPath = $avatar, IsOnline = $online, LastSeen = $lastSeen;
            ";
            command.Parameters.AddWithValue("$id", peer.Id);
            command.Parameters.AddWithValue("$name", peer.Name);
            command.Parameters.AddWithValue("$ip", peer.IpAddress);
            command.Parameters.AddWithValue("$port", peer.Port);
            command.Parameters.AddWithValue("$avatar", (object?)peer.AvatarPath ?? DBNull.Value);
            command.Parameters.AddWithValue("$online", peer.IsOnline ? 1 : 0);
            command.Parameters.AddWithValue("$lastSeen", peer.LastSeen.ToString("O"));
            command.ExecuteNonQuery();
        }

        public List<Peer> GetAll()
        {
            var result = new List<Peer>();
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, IpAddress, Port, AvatarPath, IsOnline, LastSeen FROM Peers;";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Peer
                {
                    Id = reader.GetString(0),
                    Name = reader.GetString(1),
                    IpAddress = reader.GetString(2),
                    Port = reader.GetInt32(3),
                    AvatarPath = reader.IsDBNull(4) ? null : reader.GetString(4),
                    IsOnline = reader.GetInt32(5) == 1,
                    LastSeen = DateTime.Parse(reader.GetString(6))
                });
            }
            return result;
        }

        public void Delete(string peerId)
        {
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Peers WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", peerId);
            command.ExecuteNonQuery();
        }
    }
}