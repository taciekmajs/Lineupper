using Lineupper.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Services.Interfaces
{
    public interface IScheduleItemService
    {
        Task<IEnumerable<ScheduleItemDto>> GetAllAsync();
        Task<ScheduleItemDto?> GetByIdAsync(Guid id);
        Task<ScheduleItemDto> CreateAsync(ScheduleItemDto itemDto);
        Task<ScheduleItemDto?> UpdateAsync(Guid id, ScheduleItemDto itemDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
