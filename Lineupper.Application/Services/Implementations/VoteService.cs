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
    public class VoteService : IVoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VoteDto>> GetAllAsync()
        {
            var votes = await _unitOfWork.Votes.GetAllAsync();
            return _mapper.Map<IEnumerable<VoteDto>>(votes);
        }

        public async Task<VoteDto?> GetByIdAsync(Guid id)
        {
            var vote = await _unitOfWork.Votes.GetByIdAsync(id);
            return _mapper.Map<VoteDto>(vote);
        }

        public async Task<VoteDto> CreateAsync(VoteDto voteDto)
        {
            var vote = _mapper.Map<Vote>(voteDto);
            await _unitOfWork.Votes.AddAsync(vote);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VoteDto>(vote);
        }

        public async Task<VoteDto?> UpdateAsync(Guid id, VoteDto voteDto)
        {
            var existing = await _unitOfWork.Votes.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(voteDto, existing);
            _unitOfWork.Votes.Update(existing);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VoteDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var vote = await _unitOfWork.Votes.GetByIdAsync(id);
            if (vote == null) return false;

            _unitOfWork.Votes.Remove(vote);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
