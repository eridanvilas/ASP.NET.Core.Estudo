using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Domain.Model.Entities;
using WebAPI.Identity.Models;

namespace WebAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //GET: api/Role
       [HttpGet]
         public IActionResult Get()
            {
            return Ok(new
            {
                role = new RoleDTO(),
                UpdateUserRole = new UpdateUserRoleDTO()
            });
            }

        // GET: api/Role/5
        // [HttpGet("{id}", Name = "Get")]
        // public string Get(int id)
        // {
        //     return "value";
        // }

        // POST: api/Role
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(RoleDTO roleDTO)
        {
            try
            {
                var result = await _roleManager.CreateAsync(new Role { Name = roleDTO.Name });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error {ex.Message}");
            }
        }
        [HttpPut("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleDTO model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return StatusCode(StatusCodes.Status404NotFound, $"ERROR USER NOT FOUND");

                if (model.Delete)
                    await _userManager.RemoveFromRoleAsync(user, model.Role);

                else
                    await _userManager.AddToRoleAsync(user, model.Role);

                return Ok("Success");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"ERROR {ex.Message}");

            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
