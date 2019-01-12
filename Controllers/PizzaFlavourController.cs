using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiExample.Services;

namespace WebApiExample.Controllers
{
    public class PizzaFlavourController : ControllerBase
    {
        private readonly IPizzaFlavourRepositoryService flavourService;

        public PizzaFlavourController(IPizzaFlavourRepositoryService flavourService)
        {
            this.flavourService = flavourService;
        }

        public ActionResult<IEnumerable<string>> GetFlavourNames()
        {
            return Ok(this.flavourService.GetFlavours().Select(f => f.Value));
        }

        public ActionResult<IEnumerable<string>> FindFlavour(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest($"The parameter '{nameof(id)}' shall be provided.");
            }

            if (!this.flavourService.TryGetIngredients(id, out IEnumerable<string> ingredients))
            {
                return NotFound();
            }

            return Ok(ingredients);
        }

        public ActionResult<int> Post(AddPizzaFlavourRequest request)
        {
            if (string.IsNullOrEmpty(request.FlavourName))
            {
                return BadRequest($"The parameter 'FlavourName' shall be provided.");
            }

            var ingredients = request.Ingredients.Where(i => !string.IsNullOrEmpty(i));
            int flavourId = this.flavourService.AddNewFlavour(request.FlavourName, ingredients);

            return Ok(flavourId);
        }
    }

    public class AddPizzaFlavourRequest
    {
        public string FlavourName { get; set; }

        public IEnumerable<string> Ingredients { get; set; }

        public AddPizzaFlavourRequest()
        {
            
        }

        public AddPizzaFlavourRequest(string flavourName, IEnumerable<string> ingredients)
        {
            this.FlavourName = flavourName;
            this.Ingredients = new List<string>(ingredients);
        }
    }
}
