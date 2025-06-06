using Lineupper.SharedKernel.Enums;

namespace Lineupper.Domain.Models
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public UserType UserType { get; set; }
    }
}
