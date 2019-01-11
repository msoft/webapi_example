using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiExample.Services;

namespace WebApiExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaOrderController : ControllerBase
    {
        private readonly IPizzaFlavourRepositoryService flavourService;
        private readonly IPizzaOrderRepositoryService orderService;

        public PizzaOrderController(IPizzaFlavourRepositoryService flavourService,
            IPizzaOrderRepositoryService orderService)
        {
            this.flavourService = flavourService;
            this.orderService = orderService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderedPizza>> GetOrderedPizzas()
        {
            var pizzaOrders = this.orderService.GetOrders();
            var orderedPizzas = pizzaOrders.Select(p => new OrderedPizza(
                p.Id,
                this.flavourService.GetFlavours()[p.FlavourId],
                p.CreationDate
            ));

            return Ok(orderedPizzas);
        }

        [HttpPost("{pizzaFlavour}")]
        public ActionResult<int> AddNewOrder(string pizzaFlavour)
        {
            if (string.IsNullOrEmpty(pizzaFlavour))
            {
                return BadRequest($"The parameter '{nameof(pizzaFlavour)}' shall be provided.");
            }

            if (!this.flavourService.TryGetFlavour(pizzaFlavour, out PizzaFlavour flavour))
            {
                return BadRequest($"The flavour '{pizzaFlavour}' is unknown.");
            }

            var orderId = this.orderService.AddNewOrder(flavour.Id);

            return Ok(orderId);
        }
    }

    public class OrderedPizza
    {
        public int OrderId { get; set; }

        public string PizzaFlavour { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderedPizza(int orderId, string pizzaFlavour, DateTime orderDate)
        {
            this.OrderId = orderId;
            this.PizzaFlavour = pizzaFlavour;
            this.OrderDate = orderDate;
        }
    }
}
