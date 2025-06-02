namespace Lineupper.Domain.Models
{
    public class Band
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Genre { get; set; } = default!;

        public Guid FestivalId { get; set; }
        public Festival Festival { get; set; } = default!;

        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<ScheduleItem> ScheduleItems { get; set; } = new List<ScheduleItem>();
    }
}
