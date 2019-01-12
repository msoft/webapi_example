using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiExample.Services;

namespace WebApiExample.Controllers
{    
    [Route("admin/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IPizzaOrderRepositoryService orderService;

        public AdminController(IPizzaOrderRepositoryService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<int>> GetOrders()
        {
            return Ok(this.orderService.GetOrders().Select(o => o.Id));
        }

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