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
        private readonly IFestivalRepository _festivalRepository;

        public FestivalService(IUnitOfWork unitOfWork, IMapper mapper, IFestivalRepository festivalRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _festivalRepository = festivalRepository;
        }

        public async Task<IEnumerable<FestivalDto>> GetAllAsync()
        {
            var festivals = await _unitOfWork.Festivals.GetAllAsync();
            return _mapper.Map<IEnumerable<FestivalDto>>(festivals);
        }

        public async Task<FestivalDto?> GetByIdAsync(Guid id)
        {
            var festival = await _unitOfWork.Festivals.GetByIdAsync(id);
            var bands = await _unitOfWork.Bands.GetByFestivalIdAsync(id);
            festival.Bands = (ICollection<Band>)bands;
            var mapped = _mapper.Map<FestivalDto>(festival);
            return mapped;
        }

        public async Task CreateAsync(FestivalDto festivalDto)
        {
            try
            {
                var festival = _mapper.Map<Festival>(festivalDto);
                festival.Id = Guid.NewGuid();

                List<Band> bands = new List<Band>();
                foreach (var b in festivalDto.Bands)
                {
                    var band = _mapper.Map<Band>(b);
                    band.FestivalId = festival.Id;
                    band.Id = Guid.NewGuid();
                    bands.Add(band);
                }
                festival.Bands = bands;
                await _unitOfWork.Festivals.AddAsync(festival);
                foreach (var b in bands)
                {
                    b.FestivalId = festival.Id;
                    await _unitOfWork.Bands.AddAsync(b);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
            }
        }

        public async Task<IEnumerable<Festival>> GetFestivalsByOrganizer(Guid organizerId)
        {
            var festivals = await _unitOfWork.Festivals.GetByOrganizerIdAsync(organizerId);
            return festivals;
        }

        public async Task DeleteFestival(Guid festivalId)
        {
            var festival = await _unitOfWork.Festivals.GetByIdAsync(festivalId);
            if (festival != null)
            {
                _unitOfWork.Festivals.Remove(festival);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ICollection<ScheduleItem>> GenerateScheduleForFestival(Guid festivalId)
        {
            var schedule = await _festivalRepository.GenerateScheduleForFestival(festivalId);
            return schedule;
        }

        public async Task<ICollection<ScheduleItem>> GetScheduleItems(Guid festivalId)
        {
            var schedule = await _festivalRepository.GetScheduleItems(festivalId);
            return schedule;
        }
    }
}
