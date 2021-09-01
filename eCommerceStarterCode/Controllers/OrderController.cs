using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly AppContext appcontext;
    private ResponseObject response;

    public OrderController(AppContext appDbContext)
    {
        this.appcontext = appDbContext;
        response = new ResponseObject();
        response.Status = false;
    }

    // GET: api/Orders
    [HttpGet]
    public async Task<ResponseObject> Get()
    {
        await HttpContext.Session.LoadAsync();
        var userId = HttpContext.Session.GetInt32("UserID");
        if (HttpContext.Session.GetInt32("IsLoggedIn") == 1)
        {
            List<Order> orders = await appcontext.Orders
                .Where(o => o.UserId == userId)
                .Include(oi => oi.OrderItems)
                .ToListAsync();
            response.SetContent(true, "Order history fetched successfully", orders.ToList<object>());
        }
        else
        {
            response.Status = false;
            response.Message = "Unauthorized access not allowed";
        }
        return response;
    }
    internal class ResponseObject
    {
        internal bool Status;

        public string Message { get; internal set; }

        internal void SetContent(bool v1, string v2, List<object> list)
        {
            throw new NotImplementedException();
        }
    }