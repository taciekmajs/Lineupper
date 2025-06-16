using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Dto
{
    public class SubmitVotesDto
    {
        public Guid userId { get; set; }
        public Guid festivalID { get; set; }
        public Dictionary<Guid, int> Votes { get; set; }
    }
}
