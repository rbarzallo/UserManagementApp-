using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using UserManagement.Api.DTOs;

namespace UserManagement.Api.Data
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_GetUserByEmail", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new UserDto
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    TwoFactorSecret = reader.IsDBNull(reader.GetOrdinal("TwoFactorSecret")) ? null : reader.GetString(reader.GetOrdinal("TwoFactorSecret"))
                };
            }
            return null;
        }

        public async Task RegisterUser(string username, string email, string passwordHash, string twoFactorSecret = null)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_RegisterUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
            if (twoFactorSecret != null)
                cmd.Parameters.AddWithValue("@TwoFactorSecret", twoFactorSecret);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
