using AutoMapper;
using Lineupper.Application.Dto;
using Lineupper.Application.Services.Interfaces;
using Lineupper.Domain.Contracts;
using Lineupper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Services.Implementations
{
    public class ScheduleItemService : IScheduleItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScheduleItemDto>> GetAllAsync()
        {
            var items = await _unitOfWork.ScheduleItems.GetAllAsync();
            return _mapper.Map<IEnumerable<ScheduleItemDto>>(items);
        }

        public async Task<ScheduleItemDto?> GetByIdAsync(Guid id)
        {
            var item = await _unitOfWork.ScheduleItems.GetByIdAsync(id);
            return _mapper.Map<ScheduleItemDto>(item);
        }

        public async Task<ScheduleItemDto> CreateAsync(ScheduleItemDto itemDto)
        {
            var item = _mapper.Map<ScheduleItem>(itemDto);
            await _unitOfWork.ScheduleItems.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ScheduleItemDto>(item);
        }

        public async Task<ScheduleItemDto?> UpdateAsync(Guid id, ScheduleItemDto itemDto)
        {
            var existing = await _unitOfWork.ScheduleItems.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(itemDto, existing);
            _unitOfWork.ScheduleItems.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ScheduleItemDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _unitOfWork.ScheduleItems.GetByIdAsync(id);
            if (item == null) return false;

            _unitOfWork.ScheduleItems.Remove(item);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
