using Microsoft.AspNetCore.Mvc;
using ProjectIIRESTNETCoreService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectIIRESTNETCoreService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static int currentId = 1;
        private static List<User> users = new List<User>();

        // GET: api/Users
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var user = users.FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return Ok(user);
            }
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult Post([FromBody] User value)
        {
            if (value == null)
            {
                return new BadRequestResult();
            }

            if (value.Email == null)
            {
                return BadRequest(new ErrorResponse { Message = "Email field is null", DBCode = 120, Data = value });
            }
            if (value.Password == null)
            {
                return BadRequest(new ErrorResponse { Message = "Password field is null", DBCode = 120, Data = value });
            }
            value.Id = currentId;
            currentId++;
            value.CreatedDate = DateTime.Now;
            users.Add(value);
            return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User value)
        {
            var user = users.FirstOrDefault(t => t.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = value.Email;
            user.Password = value.Password;

            return Ok(user);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var usersDeleted = users.RemoveAll(t => t.Id == id);
            if (usersDeleted == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }
    }
}
