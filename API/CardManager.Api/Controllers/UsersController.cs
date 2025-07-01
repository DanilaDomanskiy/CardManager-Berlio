using CardManager.Api.Contracts.User;
using CardManager.Application.DTOs.User;
using CardManager.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardManager.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest model, CancellationToken cancellationToken)
        {
            var userId = await _usersService.RegisterAsync(new RegisterUserDto
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                IsAdmin = model.IsAdmin,
            }, cancellationToken);

            return userId is null ? StatusCode(409) : Ok(userId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest user, CancellationToken cancellationToken)
        {
            var token = await _usersService.LoginAsync(new LoginUserDto
            {
                Email = user.Login,
                Password = user.Password
            }, cancellationToken);

            if (token is null) return StatusCode(409);

            Response.Cookies.Append("AppCookie", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMonths(1)
            });

            return NoContent();
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("AppCookie"))
            {
                Response.Cookies.Delete("AppCookie");
            }

            return NoContent();
        }

        [HttpGet("isAdmin")]
        public IActionResult IsAdmin()
        {
            var isAdmin = User.IsInRole("Admin");
            return Ok(new { isAdmin });
        }
    }
}