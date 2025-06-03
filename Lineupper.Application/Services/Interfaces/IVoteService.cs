using Lineupper.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Services.Interfaces
{
    public interface IVoteService
    {
        Task<IEnumerable<VoteDto>> GetAllAsync();
        Task<VoteDto?> GetByIdAsync(Guid id);
        Task<VoteDto> CreateAsync(VoteDto voteDto);
        Task<VoteDto?> UpdateAsync(Guid id, VoteDto voteDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
