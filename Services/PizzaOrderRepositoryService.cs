using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebApiExample.Services
{
    public interface IPizzaOrderRepositoryService
    {
        IEnumerable<PizzaOrder> GetOrders();

        int AddNewOrder(int flavourId);

        bool DeleteOrder(int orderId);
    }

    public class PizzaOrderRepositoryService : IPizzaOrderRepositoryService
    {
        private readonly IPizzaFlavourRepositoryService flavourService;
        private readonly List<PizzaOrder> orders = new List<PizzaOrder>();

        public PizzaOrderRepositoryService(IPizzaFlavourRepositoryService flavourService)
        {
            this.flavourService = flavourService;
        }

        public IEnumerable<PizzaOrder> GetOrders()
        {
            return this.orders;
        }

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

    public class PizzaOrder
    {
        public int Id { get; set; }

        public int FlavourId { get; set; }

        public DateTime CreationDate { get; set; }

        public PizzaOrder(int id, int flavourId, DateTime creationDate)
        {
            this.Id = id;
            this.FlavourId = flavourId;
            this.CreationDate = creationDate;
        }
    }
}