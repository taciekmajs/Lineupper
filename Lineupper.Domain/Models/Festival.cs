using Lineupper.SharedKernel.Enums;

namespace Lineupper.Domain.Models
{
    public class Festival
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public FestivalStatus Status { get; set; }
        public TimeSpan ConcertStartTime { get; set; }
        public TimeSpan ConcertEndTime { get; set; }

        public Guid OrganizerId { get; set; }
        public Organizer Organizer { get; set; } = default!;

        public ICollection<Band> Bands { get; set; } = new List<Band>();
        public ICollection<ScheduleItem> Schedule { get; set; } = new List<ScheduleItem>();
    }
}
