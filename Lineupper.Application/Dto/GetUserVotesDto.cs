using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Dto
{
    public class GetUserVotesDto
    {
        public Guid userId {  get; set; }
        public Guid festivalId { get; set; }
    }
}
