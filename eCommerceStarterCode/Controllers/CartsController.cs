using eCommerceStarterCode.Data;
using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllCarts()
        {
            var carts = _context.Carts;
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public IActionResult GetCartbyId(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetCartbyUser(string id)
        {
            var user = _context.Users.Find(id);
            var userCart = _context.Carts.Where(x => x.User == user);

            return Ok(userCart);
        }

        [HttpPost]
        public IActionResult NewCart([FromBody] Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
            return StatusCode(201, cart);
        }

        [HttpPut]
        public IActionResult UpdateCart([FromBody] Cart cart)
        {
            Cart cartToChange = _context.Carts.Find(cart.CartId);
            if (cartToChange != null)
            {
                return NotFound();
            }
            cartToChange = cart;
            _context.SaveChanges();
            return Ok(cart);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id)
        {
            var cartToDelete = _context.Carts.Find(id);
            if (cartToDelete == null)
            {
                return NotFound();
            }
            _context.Carts.Remove(cartToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}
