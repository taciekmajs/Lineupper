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
    public class OrganizerService : IOrganizerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrganizerRepository _organizerRepository;

        public OrganizerService(IUnitOfWork unitOfWork, IMapper mapper, IOrganizerRepository organizerRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _organizerRepository = organizerRepository;
        }

        public async Task<IEnumerable<OrganizerDto>> GetAllAsync()
        {
            var organizers = await _unitOfWork.Organizers.GetAllAsync();
            return _mapper.Map<IEnumerable<OrganizerDto>>(organizers);
        }

        public async Task<OrganizerDto?> GetByIdAsync(Guid id)
        {
            var organizer = await _unitOfWork.Organizers.GetByIdAsync(id);
            return _mapper.Map<OrganizerDto>(organizer);
        }

        public async Task<OrganizerDto> CreateAsync(OrganizerDto organizerDto)
        {
            var organizer = _mapper.Map<Organizer>(organizerDto);
            await _unitOfWork.Organizers.AddAsync(organizer);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrganizerDto>(organizer);
        }

        public async Task<OrganizerDto?> UpdateAsync(Guid id, OrganizerDto organizerDto)
        {
            var existing = await _unitOfWork.Organizers.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(organizerDto, existing);
            _unitOfWork.Organizers.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrganizerDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var deleted = _organizerRepository.DeleteOrganzier(id);


            return true;
        }
    }
}
