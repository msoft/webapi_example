using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiExample.Services;

namespace WebApiExample.Controllers
{    
    /// <summary>
    /// Admin controller
    /// </summary>
    [Route("admin/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IPizzaOrderRepositoryService orderService;

        /// <summary>
        /// Creates a new instance of <see cref="AdminController" />
        /// </summary>
        /// <param name="orderService"></param>
        public AdminController(IPizzaOrderRepositoryService orderService)
        {
            this.orderService = orderService;
        }


        /// <summary>
        /// Returns pizza order IDs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<int>> GetOrders()
        {
            return Ok(this.orderService.GetOrders().Select(o => o.Id));
        }

        /// <summary>
        /// Deletes an order from its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int id)
        {
            if (this.orderService.DeleteOrder(id))
                return Ok();
            else
                return NotFound();
        }
    }    
}