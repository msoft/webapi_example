using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        private const string Ham = "Ham";
        private const string Cheese = "Cheese";
        private const string Mushrooms = "Mushrooms";
        private const string TomatoSauce = "TomatoSauce";
        private const string Pepper = "Pepper";
        private const string Artichokes = "Artichokes";

        private readonly List<PizzaFlavour> flavours = new List<PizzaFlavour>();

        /// <summary>
        /// Creates a new instance of <see cref="PizzaFlavourRepositoryService" />
        /// </summary>
        public PizzaFlavourRepositoryService()
        {
            flavours.Add(new PizzaFlavour(1, "Regina", 
                new List<string> { Ham, Cheese, Mushrooms, TomatoSauce}));
            flavours.Add(new PizzaFlavour(2, "Margarita", 
                new List<string> { Cheese, TomatoSauce }));
            flavours.Add(new PizzaFlavour(3, "Quattro Stagioni", 
                new List<string> { Cheese, TomatoSauce, Mushrooms, Ham, Artichokes, Pepper }));
        }

        /// <summary>
        /// Returns the list of pizza flavours stored by ID
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, string> GetFlavours()
        {
            return this.flavours.ToDictionary(f => f.Id, f => f.Name);
        }

        /// <summary>
        /// Finds a pizza flavour from its name
        /// </summary>
        /// <param name="flavourName"></param>
        /// <param name="flavour"></param>
        /// <returns></returns>
        public bool TryGetFlavour(string flavourName, out PizzaFlavour flavour)
        {
            flavour = this.flavours.FirstOrDefault(f => f.Name.Equals(flavourName));
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
            int maxId = this.flavours.Select(f => f.Id).DefaultIfEmpty(0).Max();
            int newId = maxId + 1;

            this.flavours.Add(new PizzaFlavour(newId, flavourName, ingredients.ToList()));

            return newId;
        }
    }

    /// <summary>
    /// Pizza flavour
    /// </summary>
    public class PizzaFlavour
    {
        /// <summary>
        /// Gets the flavour ID
        /// </summary>
        /// <returns></returns>
        public int Id { get; set; }

        /// <summary>
        /// Gets flavour name
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// Gets the list of ingredients
        /// </summary>
        /// <returns></returns>
        public IList<string> Ingredients { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="PizzaFlavour" />
        /// </summary>
        public PizzaFlavour()
        {
            
        }

        /// <summary>
        /// Creates a new instance of <see cref="PizzaFlavour" />
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="ingredients"></param>
        public PizzaFlavour(int id, string name, IList<string> ingredients)
        {
            this.Id = id;
            this.Name = name;
            this.Ingredients = new List<string>(ingredients);
        }
    }
}