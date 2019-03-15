using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiExample.Services;
using WebApiExample.Interceptors;

namespace WebApiExample.Controllers
{
    /// <summary>
    /// Pizza order controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaOrderController : ControllerBase
    {
        private readonly IPizzaFlavourRepositoryService flavourService;
        private readonly IPizzaOrderRepositoryService orderService;

        /// <summary>
        /// Creates a new instance of <see cref="PizzaOrderController" />
        /// </summary>
        /// <param name="flavourService"></param>
        /// <param name="orderService"></param>
        public PizzaOrderController(IPizzaFlavourRepositoryService flavourService,
            IPizzaOrderRepositoryService orderService)
        {
            this.flavourService = flavourService;
            this.orderService = orderService;
        }

        /// <summary>
        /// Returns the list of ordered pizza
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Adds a new pizza order using the pizza flavour name
        /// </summary>
        /// <param name="pizzaFlavour"></param>
        /// <returns></returns>
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

    /// <summary>
    /// Ordered pizza
    /// </summary>
    public class OrderedPizza
    {
        /// <summary>
        /// Gets the order ID
        /// </summary>
        /// <returns></returns>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets the pizza flavour
        /// </summary>
        /// <returns></returns>
        public string PizzaFlavour { get; set; }

        /// <summary>
        /// Gets the order date
        /// </summary>
        /// <returns></returns>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="PizzaOrder" />
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="pizzaFlavour"></param>
        /// <param name="orderDate"></param>        
        public OrderedPizza(int orderId, string pizzaFlavour, DateTime orderDate)
        {
            this.OrderId = orderId;
            this.PizzaFlavour = pizzaFlavour;
            this.OrderDate = orderDate;
        }
    }
}
