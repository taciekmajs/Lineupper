
namespace Lineupper.Domain.Models
{
    public class Vote
    {
        public Guid Id { get; set; }

        public Guid ParticipantId { get; set; }
        public Participant Participant { get; set; } = default!;

        public Guid BandId { get; set; }
        public Band Band { get; set; } = default!;

        public Guid FestivalId { get; set; }

        public int Value { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
