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
    public class ParticipantService : IParticipantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParticipantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParticipantDto>> GetAllAsync()
        {
            var participants = await _unitOfWork.Participants.GetAllAsync();
            return _mapper.Map<IEnumerable<ParticipantDto>>(participants);
        }

        public async Task<ParticipantDto?> GetByIdAsync(Guid id)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(id);
            return _mapper.Map<ParticipantDto>(participant);
        }

        public async Task<ParticipantDto> CreateAsync(ParticipantDto participantDto)
        {
            var participant = _mapper.Map<Participant>(participantDto);
            await _unitOfWork.Participants.AddAsync(participant);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ParticipantDto>(participant);
        }

        public async Task<ParticipantDto?> UpdateAsync(Guid id, ParticipantDto participantDto)
        {
            var existing = await _unitOfWork.Participants.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(participantDto, existing);
            _unitOfWork.Participants.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ParticipantDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(id);
            if (participant == null) return false;

            _unitOfWork.Participants.Remove(participant);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
