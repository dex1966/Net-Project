using Microsoft.Data.Sqlite;

namespace ChatP2P.Data
{
    public class AppDbContext
    {
        private readonly string _connectionString;

        public AppDbContext(string dbPath = "chatp2p_local.db")
        {
            _connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }

        public SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        private void InitializeDatabase()
        {
            using var connection = CreateConnection();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Peers (
                    Id TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    IpAddress TEXT NOT NULL,
                    Port INTEGER NOT NULL,
                    AvatarPath TEXT,
                    IsOnline INTEGER NOT NULL DEFAULT 0,
                    LastSeen TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Groups (
                    Id TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    AvatarPath TEXT,
                    CreatedAt TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS GroupMembers (
                    GroupId TEXT NOT NULL,
                    PeerId TEXT NOT NULL,
                    PRIMARY KEY (GroupId, PeerId),
                    FOREIGN KEY (GroupId) REFERENCES Groups(Id),
                    FOREIGN KEY (PeerId) REFERENCES Peers(Id)
                );

                CREATE TABLE IF NOT EXISTS Messages (
                    Id TEXT PRIMARY KEY,
                    Type TEXT NOT NULL,
                    SenderId TEXT NOT NULL,
                    ReceiverId TEXT,
                    GroupId TEXT,
                    Content TEXT NOT NULL,
                    ReplyToId TEXT,
                    ForwardedFromId TEXT,
                    Timestamp TEXT NOT NULL,
                    IsRead INTEGER NOT NULL DEFAULT 0
                );

                CREATE TABLE IF NOT EXISTS Accounts (
                Id TEXT PRIMARY KEY,
                Username TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                Salt TEXT NOT NULL,
                CreatedAt TEXT NOT NULL
                );

                CREATE INDEX IF NOT EXISTS idx_messages_receiver ON Messages(ReceiverId);
                CREATE INDEX IF NOT EXISTS idx_messages_group ON Messages(GroupId);
                CREATE INDEX IF NOT EXISTS idx_messages_timestamp ON Messages(Timestamp);
            ";
            command.ExecuteNonQuery();
        }
    }
}