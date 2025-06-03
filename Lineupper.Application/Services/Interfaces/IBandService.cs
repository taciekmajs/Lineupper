using Lineupper.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Services.Interfaces
{
    public interface IBandService
    {
        Task<IEnumerable<BandDto>> GetAllAsync();
        Task<BandDto?> GetByIdAsync(Guid id);
        Task<BandDto> CreateAsync(BandDto bandDto);
        Task<BandDto?> UpdateAsync(Guid id, BandDto bandDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
