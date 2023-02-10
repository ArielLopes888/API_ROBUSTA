using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cor.Exceptions;
using Domain.Entities;
using Infra.Interfaces;
using Moq;
using Services.DTO;
using Services.Interfaces;
using Services.Services;
using Xunit;

namespace _6_Testes.Services
{
    public class UserServicesTest
    {

        [Fact]
        public async Task Create_GivenValidUserDto_ShouldCreateUser()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockUserRepository = new Mock<IUserRepository>();
            var userService = new UserServices(mockMapper.Object, mockUserRepository.Object);
            var userDto = new UserDto { Email = "test@test.com" };

            // Act
            var result = await userService.Create(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto.Email, result.Email);
        }

        [Fact]
        public async Task Create_GivenUserWithExistingEmail_ShouldThrowDomainException()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(m => m.GetByEmail(It.IsAny<string>())).ReturnsAsync(new User());
            var userService = new UserServices(mockMapper.Object, mockUserRepository.Object);
            var userDto = new UserDto { Email = "test@test.com" };

            // Act
            var exception = await Assert.ThrowsAsync<DomainException>(() => userService.Create(userDto));

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Já existe um usuário cadastrado com esse email", exception.Message);
        }

        [Fact]
        public async Task Update_GivenValidUserDto_ShouldUpdateUser()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(m => m.Get(It.IsAny<int>())).ReturnsAsync(new User { Email = "old@test.com" });
            var userService = new UserServices(mockMapper.Object, mockUserRepository.Object);
            var userDto = new UserDto { Email = "new@test.com" };

            // Act
            var result = await userService.Update(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto.Email, result.Email);
        }

        [Fact]
        public async Task Update_GivenUserWithNonExistentId_ShouldThrowDomainException()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(m => m.Get(It.IsAny<int>())).ReturnsAsync(() => null);
            var userService = new UserServices(mockMapper.Object, mockUserRepository.Object);
            var userDto = new UserDto { Email = "new@test.com" };

            // Act
            var exception = await Assert.ThrowsAsync<DomainException>(() => userService.Update(userDto));

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Não existe nenhum usuário cadastrado com esse Id", exception.Message);
        }
    }
}
