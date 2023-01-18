using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.DTO;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> Create(UserDto userDto);
        Task<UserDto> Update(UserDto userDto);
        Task Remove(long id);
        Task<UserDto> Get(long id);
        Task<List<UserDto>> Get();

        Task<UserDto> GetByEmail(string email);
        Task<List<UserDto>> SearchByEmail(string email);
        Task<List<UserDto>> SearchByName(string name);
    }
}
