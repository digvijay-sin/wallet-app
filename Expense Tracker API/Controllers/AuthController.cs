using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker_API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Login() {
            return Ok("Logged in Successfully");
        }

        public async Task<IActionResult> Signup()
        {
            return Ok("Signedup in Successfully");
        }
    }
}
