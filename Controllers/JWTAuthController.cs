using MccApi.Models;
using MccApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MccApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTAuthController : ControllerBase
    {
        private readonly IAutorizationRepository _autorizationRepository;
        private readonly IConfiguration _configuration;

        public JWTAuthController(
            IAutorizationRepository autorizationRepository,
            IConfiguration configuration)
        {
            _autorizationRepository = autorizationRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // 1. Проверяем учетные данные
                var isValid = await _autorizationRepository.ValidateCredentialsAsync(
                    request.Login,
                    request.Password);

                if (!isValid)
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Неверный логин или пароль"
                    });
                }

                // 2. Получаем данные пользователя с включенными связанными объектами
                var autorization = await _autorizationRepository.GetByLoginWithDetailsAsync(request.Login);
                if (autorization == null)
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Пользователь не найден"
                    });
                }

                // 3. Генерируем JWT токен
                var token = GenerateJwtToken(autorization);

                // 4. Возвращаем ответ
                return Ok(new
                {
                    Success = true,
                    Message = "Авторизация успешна",
                    Token = token,
                    EmployeeId = autorization.EmployeeId,
                    EmployeeName = autorization.Employee?.Name ?? "Неизвестно",
                    RoleId = autorization.RoleId,
                    RoleName = autorization.Role?.RoleName ?? "Неизвестно"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Ошибка сервера: {ex.Message}"
                });
            }
        }

        private string GenerateJwtToken(Autorization autorization)
        {
            // Настройки JWT из appsettings.json
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]
                ?? "your-secret-key-min-32-chars-long-1234567890");

            var issuer = jwtSettings["Issuer"] ?? "MccApi";
            var audience = jwtSettings["Audience"] ?? "MccClient";
            var expiresHours = int.Parse(jwtSettings["ExpiresHours"] ?? "8");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, autorization.EmployeeId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, autorization.Employee?.Name ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, autorization.Role?.RoleName ?? ""),
                new Claim("EmployeeId", autorization.EmployeeId.ToString()),
                new Claim("RoleId", autorization.RoleId.ToString())
            };

            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expiresHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Эндпоинт для проверки токена
        [HttpGet("validate")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var name = User.Identity?.Name;

            return Ok(new
            {
                Message = "Токен валиден",
                EmployeeId = employeeId,
                Name = name,
                Role = role,
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false
            });
        }
    }

    
}
