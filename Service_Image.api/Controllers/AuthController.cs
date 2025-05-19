using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service_Image.Api.Domaine.Core.DTO;

namespace Service_Image.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        //pour utiliser le service de Asp.net Identity permet de gerer les utilisateurs
        private readonly UserManager<IdentityUser> _userManager;
        //Permet d'acceder a la configuration de l'application comme JwtConfig dans appsettings.json
        private readonly IConfiguration _config;
        public AuthController(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegister)
        {
            //verifie si l'utilisateur a entree des donnees valide
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //verifie si le password et confirm password sont egaux
            if (userRegister.Password != userRegister.ConfirmPassword)
            {
                return BadRequest("le password et la confirmpassword ne correspond pas");
            }
            //verifie si l'utilisateur existe
            var exist_user = await _userManager.FindByEmailAsync(userRegister.Email);
            if (exist_user != null)
            {
                return BadRequest("l'utilisateur existe");
            }
            else
            {
                var newUser = new IdentityUser
                {
                    Email = userRegister.Email,

                };
                var createUser = await _userManager.CreateAsync(newUser, userRegister.Password);
                var token = GenerateJwtToken(newUser);
                return Ok(new { token });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO login)
        {
            //verifie si l'utilisateur existe
            var existUser = await _userManager.FindByEmailAsync(login.Email);
            if (existUser == null || !await _userManager.CheckPasswordAsync(existUser, login.Password))
            {
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });
            }
            else
            {
                var token = GenerateJwtToken(existUser);
                return Ok(new { token });
            }
        }
        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtConfig:Secret"]);

            var claims = new[]
            {
            new Claim("Id", user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
