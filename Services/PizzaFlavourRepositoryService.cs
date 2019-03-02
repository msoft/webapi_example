using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiExample.Services
{
    /// <summary>
    /// Pizza flavour repository service
    /// </summary>
    public interface IPizzaFlavourRepositoryService
    {
        /// <summary>
        /// Returns the list of pizza flavours stored by ID
        /// </summary>
        /// <returns></returns>
        IDictionary<int, string> GetFlavours();

        /// <summary>
        /// Finds a pizza flavour from its name
        /// </summary>
        /// <param name="flavourName"></param>
        /// <param name="flavour"></param>
        /// <returns></returns>
        bool TryGetFlavour(string flavourName, out PizzaFlavour flavour);

        /// <summary>
        /// Finds the list of ingredients from a pizza flavour
        /// </summary>
        /// <param name="flavourName"></param>
        /// <param name="ingredients"></param>
        /// <returns></returns>
        bool TryGetIngredients(string flavourName, out IEnumerable<string> ingredients);

        /// <summary>
        /// Adds a new pizza flavour with its ingredients
        /// </summary>
        /// <param name="flavourName"></param>
        /// <param name="ingredients"></param>
        /// <returns></returns>
        int AddNewFlavour(string flavourName, IEnumerable<string> ingredients);
    }

    /// <summary>
    /// Implementation of the pizza flavour repository service
    /// </summary>
    public class PizzaFlavourRepositoryService : IPizzaFlavourRepositoryService
    {
        private readonly List<PizzaFlavour> flavours = new List<PizzaFlavour>();
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Creates a new instance of <see cref="PizzaFlavourRepositoryService" />
        /// </summary>
        public PizzaFlavourRepositoryService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns the list of pizza flavours stored by ID
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, string> GetFlavours()
        {
            return this.GetExistingFlavours().ToDictionary(f => f.Id, f => f.Name);
        }

        /// <summary>
        /// Finds a pizza flavour from its name
        /// </summary>
        /// <param name="flavourName"></param>
        /// <param name="flavour"></param>
        /// <returns></returns>
        public bool TryGetFlavour(string flavourName, out PizzaFlavour flavour)
        {
            flavour = this.GetExistingFlavours().FirstOrDefault(f => f.Name.Equals(flavourName));
            return flavour != null;
        }

        /// <summary>
        /// Finds the list of ingredients from a pizza flavour
        /// </summary>
        /// <param name="flavourName"></param>
        /// <param name="ingredients"></param>
        /// <returns></returns>
        public bool TryGetIngredients(string flavourName, 
            out IEnumerable<string> ingredients)
        {
            ingredients = null;
            if (this.TryGetFlavour(flavourName, out PizzaFlavour foundFlavour))
            {                
                ingredients = foundFlavour.Ingredients;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a new pizza flavour with its ingredients
        /// </summary>
        /// <param name="flavourName"></param>
        /// <param name="ingredients"></param>
        /// <returns></returns>
        public int AddNewFlavour(string flavourName, IEnumerable<string> ingredients)
        {            
            int maxId = this.GetExistingFlavours().Select(f => f.Id).DefaultIfEmpty(0).Max();
            int newId = maxId + 1;

            this.flavours.Add(new PizzaFlavour(newId, flavourName, ingredients.ToList()));

            return newId;
        }

        private IEnumerable<PizzaFlavour> GetExistingFlavours()
        {
            if (!this.flavours.Any())
            {
                using (var scope = this.serviceProvider.CreateScope())
                {
                    var pizzaFlavourGenerator = scope.ServiceProvider.GetRequiredService<IPizzaFlavourGeneratorService>();
                    this.flavours.AddRange(pizzaFlavourGenerator.CreateBasicFlavours());
                }
            }

            return this.flavours;
        }
    }
}