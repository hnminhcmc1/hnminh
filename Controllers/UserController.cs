using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Assignment.Business.Services;
using Assignment.UserData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Assignment.UserManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET api/user
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var user = await _userService.GetAll();
            return new ActionResult<IEnumerable<User>>(user);
        }
        // POST api/user/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                var result = await _userService.AddUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        // PUT api/user/edit
        [AllowAnonymous]
        [Authorize]
        [HttpPut("edit")]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                var result = await _userService.UpdateUser(user);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
        // POST api/user/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginRequest data)
        {
            var user = await _userService.Authenticate(data.Email, data.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}