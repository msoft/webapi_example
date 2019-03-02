using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebApiExample.Services
{
    /// <summary>
    /// Service generating pizza flavours
    /// </summary>
    public interface IPizzaFlavourGeneratorService
    {
        /// <summary>
        /// Returns the list of pizza basic flavours 
        /// </summary>
        /// <returns></returns>
        IEnumerable<PizzaFlavour> CreateBasicFlavours();
    }

    /// <summary>
    /// Implementation of the pizza flavour generator service
    /// </summary>
    public class PizzaFlavourGeneratorService : IPizzaFlavourGeneratorService
    {
        private const string Ham = "Ham";
        private const string Cheese = "Cheese";
        private const string Mushrooms = "Mushrooms";
        private const string TomatoSauce = "TomatoSauce";
        private const string Pepper = "Pepper";
        private const string Artichokes = "Artichokes";

        /// <summary>
        /// Creates a new instance of <see cref="PizzaFlavourGeneratorService" />
        /// </summary>
        public PizzaFlavourGeneratorService()
        {            
        }

        /// <summary>
        /// Returns the list of pizza basic flavours 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PizzaFlavour> CreateBasicFlavours()
        {
            return new List<PizzaFlavour>{
                new PizzaFlavour(1, "Regina", new List<string> { Ham, Cheese, Mushrooms, TomatoSauce}),
                new PizzaFlavour(2, "Margarita", new List<string> { Cheese, TomatoSauce }),
                new PizzaFlavour(3, "Quattro Stagioni", 
                    new List<string> { Cheese, TomatoSauce, Mushrooms, Ham, Artichokes, Pepper }),
            };
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