using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CSF.Charity.Application.Services;
using CSF.Charity.Application.Features.Users.Mappers;
using CSF.Charity.Domain.Identity;
using CSF.Charity.Application.Features.Users.DTOs;

namespace Kirrk.IoT.Admin.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    //[Authorize]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> logger;
        private readonly IAccountService usersService;

        public AdminController(
            ILogger<AdminController> logger,
            IAccountService usersService)
        {
            this.logger = logger;
            this.usersService = usersService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetUsers()
        {
            var userRoles = usersService.GetRoles().ToList();
            var users = await usersService.GetAllUsers();
            var dto = users.Select(u => u.ToUserDetailsDto(userRoles));
            return Ok(dto);

        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] AddUserDto model)
        {
            if (!ModelState.IsValid)
            {
                // TODO! use middleware for model validation
                return BadRequest(ModelState);
            }
            try
            {
                var userEntity = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    AssociationId = model.AssociationId
                };

                var result = await usersService.RegisterUser(userEntity, model.Password, model.Role);

                return Ok(true);
            }

            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        public async Task<ActionResult> Update([FromBody] UpdateUserDto updateUserViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // TODO! use middleware for model validation
                    return BadRequest(ModelState);
                }

                var user = await usersService.FindByNameAsync(updateUserViewModel.UserName);

                if (user == null)
                {
                    return BadRequest("user not found");
                }

                //Update user
                user.Email = updateUserViewModel.Email;

                var result = await usersService.UpdateUser(user);

                return Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<ActionResult> Delete([FromRoute] string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("userId is maleformed");
                }

                var result = await usersService.DeleteUser(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }

        }
    }
}
