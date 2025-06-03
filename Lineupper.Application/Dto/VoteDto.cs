namespace Lineupper.Application.Dto
{
    public class VoteDto
    {
        public Guid Id { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid BandId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
