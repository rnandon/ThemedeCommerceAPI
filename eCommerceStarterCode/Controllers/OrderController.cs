using eCommerceStarterCode.Data;
using eCommerceStarterCode.DataTransferObjects;
using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var products = _context.Orders;
            return Ok(products);
        }

        // <baseurl>/api/examples/user
        [HttpGet("{id}")]
        public IActionResult GetSelectedOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public IActionResult GetOrdersByCustomer(string customerId)
        {
            var customerToFind = _context.Users.Find(customerId);
            var orders = _context.Orders.Where(x => x.User == customerToFind);

            return Ok(orders);
        }

        [HttpGet("date")]
        public IActionResult GetOrdersByDate([FromBody] DateDto data)
        {
            DateTime usableDate = new DateTime(data.Year, data.Month, data.Day, data.Hours, data.Minutes, data.Seconds);
            var orders = _context.Orders.Where(x => x.Date == usableDate);

            return Ok(orders);
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromBody] Order value)
        {
            //DateTime usableDate = new DateTime(value.Year, value.Month, value.Day, value.Hours, value.Minutes, value.Seconds);
            //Order incomingOrder = new Order() { UserId = value.UserId, Date = usableDate };

            Order orderToChange = _context.Orders.Find(value.OrderId);
            if (orderToChange == null)
            {
                return NotFound();
            }
            orderToChange.UserId = value.UserId;
            orderToChange.Date = value.Date;
            _context.Update(orderToChange);
            _context.SaveChanges();
            return Ok(orderToChange);
        }

        [HttpPost]
        public IActionResult NewOrder([FromBody] OrderDto value)
        {
            DateTime usableDate = new DateTime(value.Year, value.Month, value.Day, value.Hours, value.Minutes, value.Seconds);
            Order newOrder = new Order() { UserId = value.UserId, Date = usableDate };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();
            return StatusCode(201, newOrder);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var orderToDelete = _context.Orders.Find(id);
            if (orderToDelete == null)
            {
                return NotFound();
            }
            _context.Orders.Remove(orderToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}
