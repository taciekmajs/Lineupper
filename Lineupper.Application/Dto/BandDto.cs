namespace Lineupper.Application.Dto
{
    public class BandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Genre { get; set; } = default!;
        public Guid FestivalId { get; set; }
    }
}
