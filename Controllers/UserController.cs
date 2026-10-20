using factura_api_security_project.Data;
using factura_api_security_project.Data.Entities;
using factura_api_security_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace factura_api_security_project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet()]
        public List<User>? Get()
        {
            var user = User;
            return _userService.GetAllUsers();
        }


        [HttpGet("nationalId/{nationalid}")]
        public IActionResult GetById(string nationalId)
        {
            var data = _userService.GetUserByNationalId(nationalId);
            if(data == null)
            {
                return NotFound("User was not found");
            }
            return Ok(data);    
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _userService.GetUserById(id);
            if (data == null)
            {
                return NotFound("User was not found");
            }
            return Ok(data);
        }

        [HttpPost()]
        public IActionResult AddUser(User user)
        {
            var data = _userService.InsertUser(user);
            if(data?.Id == 0)
            {
                return Conflict("User already exists!");
            }

            return Ok(data);
        }

        [HttpPatch()]
        public IActionResult UpdateUser(User user)
        {
            var data = _userService.UpdateUser(user);
            if (data?.Id == 0)
            {
                return NotFound("User was not found");
            }

            return Ok(data);
        }

        [HttpDelete("{id}")]
        public IActionResult UpdateUser(int id)
        {
            var data = _userService.DeleteUser(id);
            if (data?.Id == 0)
            {
                return NotFound("User was not found");
            }

            return Ok(data);
        }

    }
}
