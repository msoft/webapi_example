using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebApiExample.Services
{
    public interface IPizzaFlavourRepositoryService
    {
        IDictionary<int, string> GetFlavours();

        bool TryGetFlavour(string flavourName, out PizzaFlavour flavour);

        bool TryGetIngredients(string flavourName, out IEnumerable<string> ingredients);

        int AddNewFlavour(string flavourName, IEnumerable<string> ingredients);
    }

    public class PizzaFlavourRepositoryService : IPizzaFlavourRepositoryService
    {
        private const string Ham = "Ham";
        private const string Cheese = "Cheese";
        private const string Mushrooms = "Mushrooms";
        private const string TomatoSauce = "TomatoSauce";
        private const string Pepper = "Pepper";
        private const string Artichokes = "Artichokes";

        private readonly List<PizzaFlavour> flavours = new List<PizzaFlavour>();

        public PizzaFlavourRepositoryService()
        {
            flavours.Add(new PizzaFlavour(1, "Regina", 
                new List<string> { Ham, Cheese, Mushrooms, TomatoSauce}));
            flavours.Add(new PizzaFlavour(2, "Margarita", 
                new List<string> { Cheese, TomatoSauce }));
            flavours.Add(new PizzaFlavour(3, "Quattro Stagioni", 
                new List<string> { Cheese, TomatoSauce, Mushrooms, Ham, Artichokes, Pepper }));
        }

        public IDictionary<int, string> GetFlavours()
        {
            return this.flavours.ToDictionary(f => f.Id, f => f.Name);
        }

        public bool TryGetFlavour(string flavourName, out PizzaFlavour flavour)
        {
            flavour = this.flavours.FirstOrDefault(f => f.Name.Equals(flavourName));
            return flavour != null;
        }

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

        public int AddNewFlavour(string flavourName, IEnumerable<string> ingredients)
        {            
            int maxId = this.flavours.Select(f => f.Id).DefaultIfEmpty(0).Max();
            int newId = maxId + 1;

            this.flavours.Add(new PizzaFlavour(newId, flavourName, ingredients.ToList()));

            return newId;
        }
    }

    public class PizzaFlavour
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<string> Ingredients { get; set; }

        public PizzaFlavour()
        {
            
        }

        public PizzaFlavour(int id, string name, IList<string> ingredients)
        {
            this.Id = id;
            this.Name = name;
            this.Ingredients = new List<string>(ingredients);
        }
    }
}