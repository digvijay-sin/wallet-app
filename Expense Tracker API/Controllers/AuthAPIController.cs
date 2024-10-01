using Expense_Tracker_API.Service;
using Expense_Tracker_Core.Models;
using Expense_Tracker_Data.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Expense_Tracker_API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuth _repository;
        private readonly TokenGenerator _tokenService;

        public AuthAPIController(IAuth repository, TokenGenerator tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginCred cred)
        {
            var user = await _repository.AuthenticateUser(cred);
            if (user == null)
            {
                return Unauthorized();
            }
            _tokenService.User = user;     
            return Ok(_tokenService.Token);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Signup()
        {
            return Ok("Signedup in Successfully");
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> Authorization()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (userId == null || username == null)
            {
                return Unauthorized();
            }
            return Ok(new { Message = "Authorized Successfully", UserId = userId, Username = username});
        }
    }
}
