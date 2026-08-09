using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace ChatP2P.Data.Repositories
{
    public class AccountRepository
    {
        private readonly AppDbContext _dbContext;

        public AccountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Đăng ký tài khoản mới. Trả về false nếu username đã tồn tại.
        public bool Register(string username, string password)
        {
            using var connection = _dbContext.CreateConnection();

            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "SELECT COUNT(*) FROM Accounts WHERE Username = $username;";
            checkCommand.Parameters.AddWithValue("$username", username);
            var exists = (long)checkCommand.ExecuteScalar()! > 0;
            if (exists) return false;

            var salt = GenerateSalt();
            var hash = HashPassword(password, salt);

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Accounts (Id, Username, PasswordHash, Salt, CreatedAt)
                VALUES ($id, $username, $hash, $salt, $createdAt);
            ";
            command.Parameters.AddWithValue("$id", Guid.NewGuid().ToString());
            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$hash", hash);
            command.Parameters.AddWithValue("$salt", salt);
            command.Parameters.AddWithValue("$createdAt", DateTime.Now.ToString("O"));
            command.ExecuteNonQuery();

            return true;
        }

        // Kiểm tra đăng nhập. Trả về true nếu đúng username/password.
        public bool Login(string username, string password)
        {
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT PasswordHash, Salt FROM Accounts WHERE Username = $username;";
            command.Parameters.AddWithValue("$username", username);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return false; // không tìm thấy username

            var storedHash = reader.GetString(0);
            var salt = reader.GetString(1);
            var inputHash = HashPassword(password, salt);

            return storedHash == inputHash;
        }

        public bool UsernameExists(string username)
        {
            using var connection = _dbContext.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Accounts WHERE Username = $username;";
            command.Parameters.AddWithValue("$username", username);
            return (long)command.ExecuteScalar()! > 0;
        }

        private static string GenerateSalt()
        {
            var bytes = RandomNumberGenerator.GetBytes(16);
            return Convert.ToBase64String(bytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hashBytes = sha256.ComputeHash(combined);
            return Convert.ToBase64String(hashBytes);
        }
    }
}