
namespace Lineupper.Domain.Models
{
    public class Participant : User
    {
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
