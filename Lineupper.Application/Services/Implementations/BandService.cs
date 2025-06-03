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
    public class BandService : IBandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BandDto>> GetAllAsync()
        {
            var bands = await _unitOfWork.Bands.GetAllAsync();
            return _mapper.Map<IEnumerable<BandDto>>(bands);
        }

        public async Task<BandDto?> GetByIdAsync(Guid id)
        {
            var band = await _unitOfWork.Bands.GetByIdAsync(id);
            return _mapper.Map<BandDto>(band);
        }

        public async Task<BandDto> CreateAsync(BandDto bandDto)
        {
            var band = _mapper.Map<Band>(bandDto);
            await _unitOfWork.Bands.AddAsync(band);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BandDto>(band);
        }

        public async Task<BandDto?> UpdateAsync(Guid id, BandDto bandDto)
        {
            var existing = await _unitOfWork.Bands.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(bandDto, existing);
            _unitOfWork.Bands.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BandDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var band = await _unitOfWork.Bands.GetByIdAsync(id);
            if (band == null) return false;

            _unitOfWork.Bands.Remove(band);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
