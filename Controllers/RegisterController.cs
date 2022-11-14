using BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetMix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        #region Dependancy Injection
        private readonly IRegister _regester;

        public RegisterController(IRegister regester)
        {
            _regester = regester;
        }
        #endregion


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> UserRegesterAsync([FromBody] AddUserDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _regester.UserRegesterAsync(model);
            if(user == null)
                return BadRequest("invalid username");

            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _regester.GetUserByNameAsync(model.UserName);
            if (user == null)
                return NotFound("invalid username or password");

            var result = await _regester.LoginAsync(model);
            if(!result)
                return BadRequest("invalid username or password");

            return Ok(user);
        }


        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync(Guid id)
        {
            await _regester.LogoutAsync(id);
            return NoContent();
        }
    }
}
