using System.ComponentModel.DataAnnotations;

namespace GasB360_server.Models
{
    public class AuthRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
