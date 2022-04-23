using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication5.Data;
using WebApplication5.IRepository;
using WebApplication5.Models;
using WebApplication5.Services;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
       // private readonly SignInManager<ApiUser> _signInManager;

        private readonly IUnitOfWork _unitOfWork;
        // private readonly ILogger _logger;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        public AccountController(UserManager<ApiUser> userManager, /*SignInManager<ApiUser> signInManager,*/ IUnitOfWork unitOfWork, ILogger<AccountController> logger,
            IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            //_signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
       // [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"Registration attempt for {userDto.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                var result = await _userManager.CreateAsync(user,userDto.Password);
                if (!result.Succeeded)
                { 
                    var errStr = "";
                    foreach (var error in result.Errors)
                    {
                        errStr = errStr + error.Description + ". ";
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    // return BadRequest($"User Registration attempt failed. {errStr}");
                    return BadRequest(ModelState);
                }

                foreach (var role in userDto.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
                //return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation($"Login attempt for {loginDto.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                if (!await _authManager.ValidateUser(loginDto))
                {
                    return Unauthorized();
                }
                //return Accepted(new
                //{
                //    Token = await  _authManager.CreateToken(),
                //});
                return Accepted(new TokenRequest { Token = await _authManager.CreateToken(), RefreshToken = await _authManager.CreateRefreshToken() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
                return Problem($"Something went wrong in the {nameof(Login)}", statusCode: 500);
                //return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }

        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request)
        {
            var tokenRequest = await _authManager.VerifyRefreshToken(request);
            if (tokenRequest is null)
            {
                return Unauthorized();
            }

            return Ok(tokenRequest);
        }


    }
}
