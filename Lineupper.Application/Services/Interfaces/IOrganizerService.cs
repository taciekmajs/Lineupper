using Lineupper.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Services.Interfaces
{
    public interface IOrganizerService
    {
        Task<IEnumerable<OrganizerDto>> GetAllAsync();
        Task<OrganizerDto?> GetByIdAsync(Guid id);
        Task<OrganizerDto> CreateAsync(OrganizerDto organizerDto);
        Task<OrganizerDto?> UpdateAsync(Guid id, OrganizerDto organizerDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
