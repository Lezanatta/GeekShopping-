using GeekShopping.ProductAPI.Data;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GeekShopping.ProductAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManeger;
    private readonly RoleManager<IdentityRole> _roleManeger;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManeger, RoleManager<IdentityRole> roleManeger, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManeger = userManeger;
        _roleManeger = roleManeger;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    [Route("CreateRole")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExist = await _roleManeger.RoleExistsAsync(roleName);

        if(!roleExist)
        {
            var roleResult = await _roleManeger.CreateAsync(new IdentityRole(roleName));

            if(roleResult.Succeeded)
            {
                _logger.LogInformation(1, "Roles adicionada");
                return StatusCode(StatusCodes.Status200OK, new Response()
                {
                    Status = "Success", Message = $"Role {roleName} adicionada com sucesso"
                });
            }
            else
            {
                _logger.LogInformation(2, "Erro");
                return StatusCode(StatusCodes.Status400BadRequest, new Response()
                {
                    Status = "Erro",
                    Message = $"Erro ao adicionar a role {roleName}"
                });
            }
        }

        return StatusCode(StatusCodes.Status400BadRequest, new Response()
        {
            Status = "Erro",
            Message = $"Role {roleName} já existe"
        });
    }

    [HttpPost]
    [Route("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var user = await _userManeger.FindByEmailAsync(email);

        if(user is not null)
        {
            var result = await _userManeger.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation(1, $"User {user.Email} adicionada a role {roleName}");
                return StatusCode(StatusCodes.Status200OK, new Response()
                {
                    Status = "Success",
                    Message = $"Usuário {user.Email} adicionado a role {roleName}"
                });
            }
            else
            {
                _logger.LogInformation(2, $"Erro ao adicionar o usuário {user.Email} a uma role");
                return StatusCode(StatusCodes.Status400BadRequest, new Response()
                {
                    Status = "Erro",
                    Message = $"Erro ao adicionar o usuário {user.Email} a role {roleName}"
                });
            }
        }

        return BadRequest(new { error = "Não foi possível adicionar o usuário" });
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManeger.FindByNameAsync(model.UserName!);

        if (user is not null && await _userManeger.CheckPasswordAsync(user, model.Password!))
        {
            var userRoles = await _userManeger.GetRolesAsync(user);

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("id", user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidInMinutes"], out int refreshTokenValidInMinutes);

            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidInMinutes);

            user.RefreshToken = refreshToken;

            await _userManeger.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }

        return Unauthorized();
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userManeger.FindByEmailAsync(model.UserName!);

        if (userExists is not null)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Usuário já cadastrado" });

        var user = new ApplicationUser()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName
        };

        var result = await _userManeger.CreateAsync(user, model.Password!);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Erro na criação do usuário" });

        return Ok(new Response() { Status = "Success", Message = "Usuário criado com sucesso" });
    }

    [Route("refresh-token")]
    [HttpPost]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        if (tokenModel is null)
            return BadRequest("Requisição inválida");

        string? accessToken = tokenModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenModel));

        string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentNullException(nameof(tokenModel));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal is null)
            return BadRequest("Token inválido");

        string userName = principal.Identity.Name;

        var user = await _userManeger.FindByNameAsync(userName!);

        if(user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Token inválido");

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;

        await _userManeger.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken,
        });
    }

    [Route("revoke/{username}")]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManeger.FindByNameAsync(username);

        if (user is null)
            return BadRequest("Nome do usuário inválido");

        user.RefreshToken = null;

        await _userManeger.UpdateAsync(user);

        return NoContent();
    }
}
