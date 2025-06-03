using Lineupper.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<IEnumerable<ParticipantDto>> GetAllAsync();
        Task<ParticipantDto?> GetByIdAsync(Guid id);
        Task<ParticipantDto> CreateAsync(ParticipantDto participantDto);
        Task<ParticipantDto?> UpdateAsync(Guid id, ParticipantDto participantDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
