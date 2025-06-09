using Lineupper.Application.Dto;
using Lineupper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineupper.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid id);
        Task AddAsync(RegisterUserDto registerUserDto);
        Task DeleteAsync(Guid id);
        Task<User> RegisterUser(RegisterUserDto registerUserDto);
    }
}
