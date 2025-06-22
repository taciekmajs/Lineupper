using Lineupper.Domain.Models;
using Lineupper.SharedKernel.Enums;

namespace Lineupper.Application.Dto
{
    public class FestivalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public FestivalStatus Status { get; set; } = default!;
        public Guid OrganizerId { get; set; }
        public List<BandDto> Bands { get; set; }
        public TimeSpan ConcertStartTime { get; set; }
        public TimeSpan ConcertEndTime { get; set; }
        public ICollection<ScheduleItem> Schedule { get; set; }
    }
}
