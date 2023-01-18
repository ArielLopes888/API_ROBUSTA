using API.ROBUSTA.Token;
using API.ROBUSTA.Utilities;
using API.ROBUSTA.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.ROBUSTA.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IConfiguration configuration,ITokenGenerator tokenGenerator)
        {
            _configuration = configuration; 
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        [Route("/api/vi/auth/login")]
        public IActionResult Login([FromBody]LoginViewModel loginViewModel)
        {
            try
            {
                var tokenLogin = _configuration["Jwt:Login"];
                var tokenPassword = _configuration["Jwt:Password"];
                
                if (loginViewModel.Login == tokenLogin && loginViewModel.Password == tokenPassword)
                {
                    return Ok(new ResultViewModel
                    {
                        Message = "Usuário autenticado com Sucesso!",
                        Success = true,
                        Data = new 
                        { 
                            Token = _tokenGenerator.GenerateToken(), 
                            TokenExpires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:HoursToExpire"])) 
                        }
                    });
                }
                else
                {
                    return StatusCode(401, Responses.UnauthorizedErrorMessage());
                }
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }
    }
}
