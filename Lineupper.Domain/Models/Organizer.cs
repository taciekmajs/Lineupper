
namespace Lineupper.Domain.Models
{
    public class Organizer : User
    {
        public ICollection<Festival> Festivals { get; set; } = new List<Festival>();
    }
}
