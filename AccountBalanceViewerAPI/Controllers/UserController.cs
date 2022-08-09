using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Areas.Identity.Data.User;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationContext _context;
        private readonly IConfiguration Configuration;
        private RoleManager<IdentityRole> _roleManager;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public UserController(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, ApplicationContext context,
            IConfiguration configuration, RoleManager<IdentityRole> roleManager,
             TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _context = context;
            Configuration = configuration;
            _roleManager = roleManager;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [Route("UserRegister")]
        [HttpPost]
        public async Task<ActionResult> UserRegister([FromBody] UserRegistration request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).FirstOrDefault());
                }

                await using var connection = new SqlConnection(Configuration.GetConnectionString("ApplicationContextConnection"));

                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    throw new Exception("User with this Email already exists");
                }

                var newUser = new IdentityUser
                {
                    Email = request.Email,
                    UserName = request.UserName,
                };

                if (request.Type == "admin")
                {
                    var isRoleExist = await _roleManager.FindByNameAsync("Admin");
                    if (isRoleExist == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    var createdUser = await _userManager.CreateAsync(newUser, request.Password);
                    if (!createdUser.Succeeded)
                    {
                        throw new Exception(createdUser.Errors.Select(x => x.Description).FirstOrDefault());
                    }
                    await _userManager.AddToRoleAsync(newUser, "Admin");
                }
                else if (request.Type == "user")
                {
                    var isRoleExist = await _roleManager.FindByNameAsync("User");
                    if (isRoleExist == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    var createdUser = await _userManager.CreateAsync(newUser, request.Password);

                    if (!createdUser.Succeeded)
                    {
                        throw new Exception(createdUser.Errors.Select(x => x.Description).FirstOrDefault());
                    }
                    await _userManager.AddToRoleAsync(newUser, "User");
                }
                return Ok("Registration successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("UserLogin")]
        [HttpPost]
        public async Task<ActionResult> UserLogin([FromBody] UserLogin request)
        {
            try
            {
                var User = await _userManager.FindByEmailAsync(request.Email);

                if (User == null)
                {
                    throw new Exception("User does not exist");
                }

                var userValidPassword = await _userManager.CheckPasswordAsync(User, request.Password);
                if (!userValidPassword)
                {
                    throw new Exception("User/Password combination is wrong");
                }

                await using var connection = new SqlConnection(Configuration.GetConnectionString("ApplicationContextConnection"));

                var userRoles = await _userManager.GetRolesAsync(User);
                var usertokens = await GenerateAuthenticationResultsForUser(User, userRoles);

                HttpContext.Response.Cookies.Append("Token", usertokens.Token);

                return Ok(new UserLoginResults
                {
                    User = User,
                    Role = userRoles,
                    Token = usertokens.Token,
                    RefreshToken = usertokens.RefreshToken,
                    TokenExpireDate = usertokens.TokenExpireDate,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }               

        private async Task<AuthenticationResult> GenerateAuthenticationResultsForUser(IdentityUser newUser, IList<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id),
                }),

                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            foreach (var userRole in roles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, userRole));

            }

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = newUser.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.RefreshToken.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var validateToken = GetPrincipalFromToken(tokenHandler.WriteToken(token));

            if (validateToken == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Invalid Token" }
                };
            }
            var expiryDateUnix = long.Parse(validateToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                TokenExpireDate = expiryDateUnix
            };
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validateToken);
                if (!IsJwtValidSecurityAlogorithm(validateToken))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtValidSecurityAlogorithm(SecurityToken validateToken)
        {
            return (validateToken is JwtSecurityToken jwtSecurityToken) &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
