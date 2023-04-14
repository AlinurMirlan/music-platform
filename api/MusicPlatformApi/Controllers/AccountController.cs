using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicPlatformApi.Data.Entities;
using MusicPlatformApi.Models;
using MusicPlatformApi.Repositories;
using System.Text.RegularExpressions;

namespace MusicPlatformApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly IJwtTokenRepository _jwtRepo;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenRepository jwtRepo, IMapper mapper)
        {
            _userManager = userManager;
            _signinManager = signInManager;
            _jwtRepo = jwtRepo;
            _mapper = mapper;
        }

        [HttpGet("present")]
        public async Task<ActionResult<bool>> Present(string email)
        {
            if (!Regex.Match(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").Success)
                return BadRequest("Invalid email");

            User? user = await _userManager.FindByEmailAsync(email);
            return Ok(user is not null);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody]CredentialModel credential)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User? user = await _userManager.FindByEmailAsync(credential.Email);
            if (user is null)
                return NotFound("User is not found");

            var result = await _signinManager.CheckPasswordSignInAsync(user, credential.Password, false);
            if (!result.Succeeded)
                return BadRequest("Password does not match");

            JwtModel jwtModel = _jwtRepo.CreateJwt(user);
            return Created(string.Empty, jwtModel);
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody]UserModel userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = _mapper.Map<User>(userModel);
            IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
                return new BadRequestObjectResult("Failed to create user. Please check your inputs and try again.");

            JwtModel jwtModel = _jwtRepo.CreateJwt(user);
            return Created(string.Empty, jwtModel);
        }
    }
}
