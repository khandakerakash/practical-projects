using MedicineShopApplication.API.Controllers.BasicControllers;
using MedicineShopApplication.BLL.Dtos.User;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers
{
    public class UsersController : ApiBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("No data found.");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(CreateUserRequestDto userInsertDto)
        {
            var user = await _userService.AddUser(userInsertDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserDtoId }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequestDto userUpdateDto)
        {
            if(id != userUpdateDto.UserDtoId)
            {
                return BadRequest("Data mismatch");
            }
            await _userService.UpdateUser(userUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
