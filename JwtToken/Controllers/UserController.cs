using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JwtToken.Models;
using JwtToken.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Data;

namespace JwtToken.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
 
    public class UserController : ControllerBase
    {
        public IUserService _userService;
        private readonly AppSettings _appSettings;

        public UserController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }
        [HttpGet]
        public IActionResult Get()
        {
            int i = 0;
            var j = 1 / i;
            return new ObjectResult(j);
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]credentails obj)
        {
            User userObject = new User();
            DataTable  user = _userService.Authenticate(obj.UserName, obj.Password);
            if (user.Rows.Count > 0)
            {
              
                userObject.FirstName = user.Rows[0]["FirstName"].ToString();
                userObject.LastName = user.Rows[0]["LastName"].ToString();
                userObject.Id =Convert.ToInt32( user.Rows[0]["Id"]);
                userObject.UserName = user.Rows[0]["UserName"].ToString();
                userObject.Role = user.Rows[0]["Role"].ToString();

            }
            if (user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = userObject.Id,
                UserName = userObject.UserName,
                FirstName = userObject.FirstName,
                LastName = userObject.LastName,
                Role= userObject.Role,
                Token = tokenString
            });
        }
      
        [HttpGet]
        [Authorize]
        [Route("UsersRole")]
        public string GetUsersData()
        {
            return "data";
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("AdminRole")]
        public IActionResult GetAdminsData()
        {
            return new ObjectResult("Admins data");
        }

    }
}