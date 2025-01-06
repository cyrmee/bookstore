using Application.Dtos;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController(UserManager<User> userManager, IAuthenticationService authenticationService, IMapper mapper) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IMapper _mapper = mapper;

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var username = User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(username!) ?? throw new NotFoundException("User not found.");

        // Return user data
        return Ok(_mapper.Map<UserInfoDto>(user));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByNameAsync(userLoginDto.UserName);
        if (user != null
            && await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
        {
            if (await _userManager.IsLockedOutAsync(user))
                throw new UnauthorizedException("User is locked out. Please contact an administrator.");
            return Ok(new
            {
                accessToken = await _authenticationService.GenerateAccessJwtToken(user),
                accessTokenExpiration = _authenticationService.GetAccessTokenExpiration()
            });
        }
        throw new NotFoundException("User not found or incorrect password.");
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<ActionResult> SignUp([FromBody] UserSignupDto userSignupDto)
    {
        try
        {
            var userExists = await _userManager.FindByNameAsync(userSignupDto.UserName);
            if (userExists != null)
                throw new ConflictException("User already exists!");

            var user = _mapper.Map<User>(userSignupDto);

            var result = await _userManager.CreateAsync(user, userSignupDto.Password);
            await _userManager.AddToRoleAsync(user, UserRole.Customer);

            if (!result.Succeeded)
                throw new UnprocessableEntityException("User creation failed! Please check user details and try again.");

            return Ok("User created successfully!");
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    [HttpPatch("changePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] UserChangePasswordDto userChangePasswordDto)
    {
        var user = await _userManager.FindByNameAsync(userChangePasswordDto.UserName);

        if (user != null)
        {
            if (await _userManager.CheckPasswordAsync(user, userChangePasswordDto.OldPassword))
            {
                var result = await _userManager.ChangePasswordAsync(user, userChangePasswordDto.OldPassword, userChangePasswordDto.NewPassword);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new BadRequestException(errors);
                }

                return Ok();
            }
            else
                throw new ConflictException("Old password is incorrect!");
        }

        throw new NotFoundException("User not found!");
    }

    [HttpPatch("lockUser")]
    [Authorize(Roles = $"{UserRole.Admin}")]
    public async Task<ActionResult> LockUser([FromBody] UserLockDto userLockDto)
    {
        var user = await _userManager.FindByNameAsync(userLockDto.UserName);

        if (user != null)
        {
            var lockoutEndDate = DateTime.UtcNow.AddDays(userLockDto.LockoutInDays);
            var lockoutResult = await _userManager.SetLockoutEndDateAsync(user, lockoutEndDate);

            if (lockoutResult.Succeeded) return NoContent();

            throw new BadRequestException("Lockout failed!");
        }

        throw new NotFoundException("User not found!");
    }

    [HttpPatch("unlockUser")]
    [Authorize(Roles = $"{UserRole.Admin}")]
    public async Task<ActionResult> UnlockUser([FromBody] UserUnlockDto userUnlockDto)
    {
        var user = await _userManager.FindByNameAsync(userUnlockDto.UserName);

        if (user != null)
        {
            var unlockResult = await _userManager.SetLockoutEndDateAsync(user, null);

            // Optional: Perform any additional actions or revoke tokens associated with unlocking the user

            if (unlockResult.Succeeded) return NoContent();

            var errors = string.Join(", ", unlockResult.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
        }

        throw new NotFoundException("User not found!");
    }
}