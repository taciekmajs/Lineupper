using System;
using System.Collections.Generic;

namespace Lineupper.Domain.Models
{
    public class ScheduleItem
    {
        public Guid Id { get; set; }

        public Guid FestivalId { get; set; }
        public Festival Festival { get; set; } = default!;

        public Guid BandId { get; set; }
        public Band Band { get; set; } = default!;
        public string BandName { get; set; }

        public int StageNumber { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
