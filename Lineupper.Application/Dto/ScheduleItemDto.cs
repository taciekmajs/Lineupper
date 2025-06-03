namespace Lineupper.Application.Dto
{
    public class ScheduleItemDto
    {
        public Guid Id { get; set; }
        public Guid FestivalId { get; set; }
        public Guid BandId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
