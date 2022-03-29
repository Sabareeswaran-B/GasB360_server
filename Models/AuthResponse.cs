
namespace GasB360_server.Models;

    public class AuthResponse
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string? Token { get; set; }

        public AuthResponse(Guid id, string role, string token)
        {
            Id = id;
            Role = role;
            Token = token;
        }
    };

