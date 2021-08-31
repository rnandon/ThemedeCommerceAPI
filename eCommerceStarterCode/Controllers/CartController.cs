using eCommerceStarterCode.Data;
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
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        //[HttpGet]
        //public IActionResult GetAllCarts()
        //{
        //    var carts = _context.Carts;
        //    return Ok(carts);
        //}

        [HttpGet("{id}")]
        public IActionResult GetUserCart(string id)
        {
            var cart = _context.Carts.Where(ur => ur.UserId == id);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
    }
}
