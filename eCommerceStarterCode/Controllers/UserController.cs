using eCommerceStarterCode.Data;
using eCommerceStarterCode.DataTransferObjects;
using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // <baseurl>/api/user
        // Restricted to admin. Returns too much info
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult GetUser()
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            if (currentUser == null)
            {
                return NotFound();
            }
            return Ok(currentUser);
        }

        // <baseurl>/api/user
        // Only allows modification of own account.
        [HttpPut, Authorize]
        public IActionResult UpdateUser([FromBody] UserDto update)
        {
            // Get current user and requested product to change
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);

            // Check for real values
            if (currentUser == null)
            {
                return NotFound();
            }

            // Update info
            currentUser.UserName = update.UserName;
            currentUser.Email = update.Email;
            currentUser.FirstName = update.FirstName;
            currentUser.LastName = update.LastName;
            currentUser.PhoneNumber = update.PhoneNumber;

            _context.Update(currentUser);
            _context.SaveChanges();
            return Ok(update);
        }

        // <baseurl>/api/user
        // Only allows a user to delete their own account.
        [HttpDelete(), Authorize]
        public IActionResult DeleteUser()
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);

            // Check for real values
            if (currentUser == null)
            {
                return NotFound();
            }

            // Delete and save
            _context.Users.Remove(currentUser);
            _context.SaveChanges();
            return Ok();
        }
    }
}
