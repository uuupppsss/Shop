using ApiShop.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopLib;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ShopdbContext _context;

        public AuthController(ShopdbContext context)
        {
            _context = context;
        }

        [HttpGet("SignIn/{username}/{password}")]
        public async Task<ActionResult<AuthResponse>> SignIn(string username, string password)
        {
            var found_user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);
            if (found_user == null) return Unauthorized("Пользователь с таким именем не найден");
            if (found_user.Password != password) return Unauthorized("Пароль не верный");

            var claims = new List<Claim>()
            {
                new Claim(ClaimValueTypes.Integer32, found_user.Id.ToString()),
                new Claim (ClaimTypes.Role, found_user.Role.Title),
            };

            var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        //кладём полезную нагрузку
        claims: claims,
        //устанавливаем время жизни токена 30
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new AuthResponse
            {
                Token = token,
                User=new UserDTO
                {
                    Id=found_user.Id,
                    Username=username,
                    Email=found_user.Email,
                    Phone=found_user.Phone,
                    RoleId=found_user.RoleId,
                }
            });
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp(UserDTO sent_user)
        {
            User found_user = await _context.Users.FirstOrDefaultAsync(u => u.Username == sent_user.Username);
            if (found_user != null) return BadRequest("Такой логин уже существует");

            User user = new User()
            {
                Username = sent_user.Username,
                Password = sent_user.Password,
                Email = sent_user.Email,
                Phone = sent_user.Phone,
                RoleId = 2,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            if (await _context.Users.ContainsAsync(user)) return Ok();

            else return BadRequest("Что-то пошло не так");
        }
    }
}
