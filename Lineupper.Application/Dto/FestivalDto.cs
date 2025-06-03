namespace Lineupper.Application.Dto
{
    public class FestivalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = default!;
        public Guid OrganizerId { get; set; }
    }
}
