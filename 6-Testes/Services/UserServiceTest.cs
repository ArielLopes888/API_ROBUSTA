using System.Threading.Tasks;
using _6_Testes;
using AutoMapper;
using Cor.Exceptions;
using Domain.Entities;
using Infra.Interfaces;
using Moq;
using Services.DTO;
using Services.Interfaces;
using Xunit;
using FluentAssertions;

namespace Services.Services.Tests
{
    public class UserServicesTests
    {
        private readonly IUserService _sut;

        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserServicesTests()
        {
            _mapper = AutoMapperConfig.GetConfiguration();
            _userRepositoryMock = new Mock<IUserRepository>();

            _sut = new UserServices
            (
                mapper: _mapper,
                userRepository: _userRepositoryMock.Object
            );
        }

        [Fact(DisplayName = "Create Valid User")]
        public async Task Create_WhenUserIsValid_ReturnUserDTO()
        {
            //Arrange
            var userToCreate = new UserDto { Name = "Name", Email="teste@gmail.com", Password = "1234567890" };

            var userCreated = _mapper.Map<User>(userToCreate);

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>()))
               .ReturnsAsync(() => userCreated);

            //Act   
            var result = await _sut.Create(userToCreate);


            //Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDto>(userCreated));
        }
    }
}