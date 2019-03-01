using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebApiExample.Services
{
    /// <summary>
    /// Pizza order repository service
    /// </summary>
    public interface IPizzaOrderRepositoryService
    {
        /// <summary>
        /// Returns the list of orders 
        /// </summary>
        /// <returns></returns>
        IEnumerable<PizzaOrder> GetOrders();

        /// <summary>
        /// Adds a new order from a flavour ID and returns the order ID
        /// </summary>
        /// <param name="flavourId"></param>
        /// <returns></returns>
        int AddNewOrder(int flavourId);

        /// <summary>
        /// Deletes an order from its ID
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool DeleteOrder(int orderId);
    }

    /// <summary>
    /// Implementation of pizza order repository service
    /// </summary>
    public class PizzaOrderRepositoryService : IPizzaOrderRepositoryService
    {
        private readonly IPizzaFlavourRepositoryService flavourService;
        private readonly List<PizzaOrder> orders = new List<PizzaOrder>();

        /// <summary>
        /// Creates a new instance of <see cref="PizzaOrderRepositoryService" />
        /// </summary>
        /// <param name="flavourService"></param>
        public PizzaOrderRepositoryService(IPizzaFlavourRepositoryService flavourService)
        {
            this.flavourService = flavourService;
        }

        /// <summary>
        /// Returns the list of orders 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PizzaOrder> GetOrders()
        {
            return this.orders;
        }

        /// <summary>
        /// Adds a new order from a flavour ID and returns the order ID
        /// </summary>
        /// <param name="flavourId"></param>
        /// <returns></returns>
        public int AddNewOrder(int flavourId)
        {
            if (!this.flavourService.GetFlavours().ContainsKey(flavourId))
            {
                throw new InvalidOperationException($"Flavour with the ID {flavourId} doesn't exist !");
            }

            int maxId = this.orders.Select(o => o.Id).DefaultIfEmpty(0).Max();
            int newId = maxId + 1;

            this.orders.Add(new PizzaOrder(newId, flavourId, DateTime.Now));

            return newId;
        }

        /// <summary>
        /// Deletes an order from its ID
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool DeleteOrder(int orderId)
        {
            var foundOrder = this.orders.FirstOrDefault(o => o.Id.Equals(orderId));
            if (foundOrder != null) 
            {
                this.orders.Remove(foundOrder);
                return true;
            }

            return false;            
        }
    }

    /// <summary>
    /// Pizza order
    /// </summary>
    public class PizzaOrder
    {
        /// <summary>
        /// Gets the order ID
        /// </summary>
        /// <returns></returns>
        public int Id { get; set; }

        /// <summary>
        /// Gets the flavour ID
        /// </summary>
        /// <returns></returns>
        public int FlavourId { get; set; }

        /// <summary>
        /// Gets the order creation date
        /// </summary>
        /// <returns></returns>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="PizzaOrder" />
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flavourId"></param>
        /// <param name="creationDate"></param>
        public PizzaOrder(int id, int flavourId, DateTime creationDate)
        {
            this.Id = id;
            this.FlavourId = flavourId;
            this.CreationDate = creationDate;
        }
    }
}