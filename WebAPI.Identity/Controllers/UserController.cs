using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Domain.Model.Entities;
using WebAPI.Identity.Models;

namespace WebAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserController(IConfiguration config, UserManager<User> userManager,
            SignInManager<User> signInManager, IMapper mapper)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(new UserDTO());
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userLogin.UserName);
                if (user == null)
                {
                    return NotFound();
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);
                if (result.Succeeded)
                {
                    var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == user.UserName.ToUpper());
                    var userReturn = _mapper.Map<UserDTO>(appUser);

                    return Ok(new
                    {
                        token = GenerateJWToken(appUser).Result,
                        user = userReturn
                    });

                }

                return Unauthorized();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error {ex.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userDTO.UserName);

                if (user == null)
                {
                    user = new User
                    {
                        UserName = userDTO.UserName,
                        Email = userDTO.UserName,
                        FullName = userDTO.FullName
                    };

                    var result = await _userManager.CreateAsync(user, userDTO.Password);

                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.NormalizedUserName == user.UserName.ToUpper());

                        var token = GenerateJWToken(appUser).Result;
                        //var confirmationEmail = Url.Action("ConfirmEmail", "Home", new { token = token, email = user.Email }, Request.Scheme);
                        //System.IO.File.WriteAllText("confirmationEmailLink.txt", confirmationEmail);

                        //return RedirectToAction("Success");
                        return Ok(token);
                    }
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error {ex.Message}");
            }
        }

        private async Task<string> GenerateJWToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                _config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
