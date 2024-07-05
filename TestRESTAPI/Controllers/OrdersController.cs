﻿using Microsoft.AspNetCore.Http;
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

            Orders order = new()
            {
                user = orderData.user,
                photographer = orderData.photographer,
                OrderData = orderData.OrderData,
                typeOfTask = orderData.typeOfTask,
                duration = orderData.duration,
                 invoice = orderData.invoice,
                 phoneNumber =orderData.phoneNumber,
                 location = orderData.location,
                 photographerName = orderData.PhotographerName,

                
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


        [HttpGet("GetUserOrders")]
        public async Task<IActionResult> GetUser(string id)
        {
            if (ModelState.IsValid)
            {
                var MyOrders = await   _db.Orders
                        .Where(x => (x.photographer == id && x.stata == "pending") )
                                  .Select(x => x)
                                                 .ToListAsync();
                if (MyOrders == null)
                {
                    return NotFound("NO Order Yet");
                }

                return Ok(new { respone = MyOrders });


            }

            return BadRequest(ModelState);
        }


        [HttpGet("ChangeOrderStata")]
        public async Task<IActionResult> ChangeOrderStata(string id , string stata)
        {
            if (ModelState.IsValid)
            {
                var orderToUpdate = await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);

                orderToUpdate.stata = stata;
                await _db.SaveChangesAsync();

                return Ok(new { respone = "success" });


            }

            return BadRequest(ModelState);
        }


        [HttpGet("GetAcceptedUserOrders")]
        public async Task<IActionResult> GetAcceptedUserOrders(string id)
        {
            if (ModelState.IsValid)
            {
                var MyOrders = await _db.Orders
                        .Where(x => (x.photographer == id && x.stata == "accept" && x.PayStata == "Done"))
                                  .Select(x => x)
                                                 .ToListAsync();
                if (MyOrders == null)
                {
                    return NotFound("NO Order Yet");
                }

                return Ok(new { respone = MyOrders });


            }

            return BadRequest(ModelState);
        }


        [HttpGet("ReadyToPay")]
        public async Task<IActionResult> ReadyToPay(string id)
        {
            if (ModelState.IsValid)
            {
                var Myhire = await _db.Orders
                        .Where(x => ((x.user == id )&& (x.stata == "accept") && (x.PayStata == "pending")))
                                  .Select(x => x)
                                                 .ToListAsync();
                if (Myhire == null)
                {
                    return NotFound("NO hire Yet");
                }

                return Ok(new { respone = Myhire });


            }

            return BadRequest(ModelState);
        }



        [HttpGet("ChangePayStata")]
        public async Task<IActionResult> ChangePayStata(string id, string stata)
        {
            if (ModelState.IsValid)
            {
                var orderToUpdate = await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);
                orderToUpdate.PayStata = stata;
                await _db.SaveChangesAsync();

                return Ok(new { respone = "success" });


            }

            return BadRequest(ModelState);
        }
    }
}
