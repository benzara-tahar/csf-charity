
using CSF.Charity.Application.Features.Users.DTOs;
using CSF.Charity.Application.Features.Users.Mappers;
using CSF.Charity.Application.Services;
using CSF.Charity.Domain.Identity;
using CSF.Charity.Infrastructure.Identity.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kirrk.IoT.Admin.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAccountService usersService;

        public AuthController(
            ILogger<AuthController> logger,
            IOptions<JwtBearerTokenSettings> jwtTokenOptions,
            UserManager<ApplicationUser> userManager,
            IAccountService accountService)
        {
            this.logger = logger;
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
            this.userManager = userManager;
            this.usersService = accountService;
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest credentials)
        {
            try
            {
                var userRoles = usersService.GetRoles().ToList();
                ApplicationUser identityUser;
                var user = await ValidateUser(credentials);
                if (!ModelState.IsValid
                    || credentials == null
                    || (identityUser = user) == null)
                {
                    return BadRequest("Login failed");
                }

                var token = GenerateToken(identityUser);
                var res = user.ToUserLoggedInDto(userRoles, token);
                return Ok(new { user = res, Message = "Success" });
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        //TODO: implement logout
        [HttpPost]
        [AllowAnonymous]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = Ok();
            return await Task.FromResult(result);
        }

        //TODO: implement refresh token
        [HttpPost]
        [AllowAnonymous]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = Ok();
            return await Task.FromResult(result);
        }

        #region private
        private async Task<ApplicationUser> ValidateUser(LoginRequest credentials)
        {
            var identityUser = await userManager.FindByNameAsync(credentials.UserName);
            if (identityUser != null)
            {
                var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
                return result == PasswordVerificationResult.Failed ? null : identityUser;
            }

            return null;
        }

        private string GenerateToken(ApplicationUser identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                    new Claim(ClaimTypes.Email, identityUser.Email)
                }),

                //TODO: This is temporary while we implement refresh token
                //Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            if (!string.IsNullOrWhiteSpace(identityUser.AssociationId))
            {
                tokenDescriptor.Subject.AddClaim(new Claim("AssociationId", identityUser.AssociationId));
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion

    }
}
