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
    public class FestivalService : IFestivalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FestivalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FestivalDto>> GetAllAsync()
        {
            var festivals = await _unitOfWork.Festivals.GetAllAsync();
            return _mapper.Map<IEnumerable<FestivalDto>>(festivals);
        }

        public async Task<FestivalDto?> GetByIdAsync(Guid id)
        {
            var festival = await _unitOfWork.Festivals.GetByIdAsync(id);
            return _mapper.Map<FestivalDto>(festival);
        }

        public async Task CreateAsync(FestivalDto festivalDto)
        {
            var festival = _mapper.Map<Festival>(festivalDto);
            await _unitOfWork.Festivals.AddAsync(festival);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
