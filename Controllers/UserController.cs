using BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetMix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "DashBoard")]
    public class UserController : ControllerBase
    {
        #region Dependancy Injection
        private readonly IUser _users;

        public UserController(IUser users)
        {
            _users = users;
        }
        #endregion


        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _users.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddUserAsync([FromBody] AddUserDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _users.AddUserAsync(model);
            return Ok(user);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _users.GetUserAsync(model.Id);
            if (result == null)
                return NotFound("invalid id");

            var user = await _users.UpdateUserAsync(model, result);
            return Ok(user);
        } 

        [HttpPost]
        [Route("role")]
        public async Task<IActionResult> UpdateUserRoleAsync(Guid id)
        {
            var result = await _users.GetUserAsync(id);
            if(result == null)
                return NotFound("invalid id");
            var user = await _users.UpdateRole(result);
            return NoContent();
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            var user = await _users.GetUserAsync(id);
            if (user == null)
                return NotFound("invalid id");
            await _users.DeleteUserAsync(user);
            return NoContent();
        }
    }
}
