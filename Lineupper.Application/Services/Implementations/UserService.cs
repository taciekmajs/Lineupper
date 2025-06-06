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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task AddAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                _unitOfWork.Users.Remove(user);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<User> RegisterUser(RegisterUserDto registerUserDto)
        {
            if (registerUserDto.UserType == SharedKernel.Enums.UserType.Participant)
            {
                var user = _mapper.Map<Participant>(registerUserDto);
                await _unitOfWork.Participants.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return user;
            }
            else
            {
                var user = _mapper.Map<Organizer>(registerUserDto);
                await _unitOfWork.Organizers.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return user;
            }
        }
    }
}
