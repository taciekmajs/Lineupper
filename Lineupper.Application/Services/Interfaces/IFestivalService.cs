using Lineupper.Application.Dto;
using Lineupper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Services.Interfaces
{
    public interface IFestivalService
    {
        Task<IEnumerable<FestivalDto>> GetAllAsync();
        Task<FestivalDto?> GetByIdAsync(Guid id);
        Task CreateAsync(FestivalDto festivalDto);
        Task<IEnumerable<Festival>> GetFestivalsByOrganizer(Guid organizerId);
        Task DeleteFestival(Guid festivalId);
        Task<ICollection<ScheduleItem>> GenerateScheduleForFestival(Guid festivalId);
    }
}
