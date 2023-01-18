using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cor.Exceptions;
using Domain.Entities;
using Infra.Interfaces;
using Services.DTO;
using Services.Interfaces;

namespace Services.Services
{
    public class UserServices : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserServices(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserDto> Create(UserDto userDto) 
        { 
            var userExist = await _userRepository.GetByEmail(userDto.Email);

            if (userExist != null)
                throw new DomainException("Já existe um usuário cadastrado com esse email");

            var user = _mapper.Map<User>(userDto);
            user.Validate();

            var userCreated = await _userRepository.Create(user);

            return _mapper.Map<UserDto>(userCreated);
        }
        public async Task<UserDto> Update(UserDto userDto)
        {
            var userExist = await _userRepository.Get(userDto.Id);

            if (userExist == null)
                throw new DomainException("Não existe nenhum usuário cadastrado com esse Id");

            var user = _mapper.Map<User>(userDto);
            user.Validate();

            var userUpdated = await _userRepository.Update(user);

            return _mapper.Map<UserDto>(userUpdated);
        } 
            
        public async Task Remove(long id)
        {
            await _userRepository.Remove(id);
        }

        public async Task<UserDto> Get(long id)
        {
            var user = await _userRepository.Get(id);

            return _mapper.Map<UserDto>(user);
        }
        public async Task<List<UserDto>> Get()
        {
            var allUsers = await _userRepository.Get();

            return _mapper.Map<List<UserDto>>(allUsers);
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            return _mapper.Map<UserDto>(user);
        }
        public async Task<List<UserDto>> SearchByEmail(string email)
        {
            var allUsers = await _userRepository.SearchByEmail(email);

            return _mapper.Map<List<UserDto>>(allUsers);
        }
        public async Task<List<UserDto>> SearchByName(string name)
        {
            var allUsers = await _userRepository.SearchByName(name);

            return _mapper.Map<List<UserDto>>(allUsers);
        }
    }
}
