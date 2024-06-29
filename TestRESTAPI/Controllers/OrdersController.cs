using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRESTAPI.Data;
using TestRESTAPI.Data.Models;
using TestRESTAPI.Models;

namespace TestRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        private readonly AppDbContext _db;

        [HttpPost("Pay")]
        public async Task<IActionResult> AddOrder( dtoOrder orderData )
        {
            Orders order = new ()
            {
                user = orderData.user,
                photographer = orderData.photographer,
                OrderData = orderData.OrderData,
                typeOfTask= orderData.typeOfTask,
                duration= orderData.duration
            };
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            return Ok(new { respone = "Sucess" });
        }
        [HttpPost("CheckOrders")]
        public async Task<IActionResult> CheckOrders(dtoCheckOrders CheckOrder)
        {
            Orders? order = await _db.Orders.SingleOrDefaultAsync(x => x.photographer == CheckOrder.photographer
            && x.OrderData == CheckOrder.OrderData);
            if(order == null)
            {
                return Ok(new { respone = "Avalible" });
            }
            else
            {
                return Ok(new { respone = "NotAvalible" });

            }
          
        }
    }
}
