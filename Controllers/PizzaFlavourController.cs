using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApiExample.Services;

namespace WebApiExample.Controllers
{
    /// <summary>
    /// Controller used for fetching information about Pizza Flavour.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaFlavourController : ControllerBase
    {
        private readonly IPizzaFlavourRepositoryService flavourService;

        /// <summary>
        /// Creates a new instance of <see cref="PizzaFlavourController" />
        /// </summary>
        /// <param name="flavourService"></param>
        public PizzaFlavourController(IPizzaFlavourRepositoryService flavourService)
        {
            this.flavourService = flavourService;
        }

        /// <summary>
        /// Returns the list of flavour names
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetFlavourNames()
        {
            return Ok(this.flavourService.GetFlavours().Select(f => f.Value));
        }

        
        /// <summary>
        /// Find flavour using flavour name
        /// </summary>
        /// <remarks>Usefull remark</remarks>
        /// <response code="200">Flavour retreived</response>
        /// <response code="400">Flavour not found</response>
        /// <response code="500">Bad request</response>
        [HttpGet("{flavourName}", Name = "FindFlavourByName")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<string>> FindFlavour(string flavourName)
        {
            if (string.IsNullOrEmpty(flavourName))
            {
                return BadRequest($"The parameter '{nameof(flavourName)}' shall be provided.");
            }

            if (!this.flavourService.TryGetIngredients(flavourName, out IEnumerable<string> ingredients))
            {
                return NotFound();
            }

            return Ok(ingredients);
        }

        /// <summary>
        /// Creates a new pizza flavour
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddPizzaFlavour")]
        public ActionResult<int> Post([FromBody, BindRequired]AddPizzaFlavourRequest request)
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

    /// <summary>
    /// Request for adding a new pizza flavour
    /// </summary>
    public class AddPizzaFlavourRequest
    {
        /// <summary>
        /// Returns the flavour name
        /// </summary>
        /// <returns></returns>
        public string FlavourName { get; set; }

        /// <summary>
        /// Returns the list of ingredients
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Ingredients { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="AddPizzaFlavourRequest" />
        /// </summary>
        public AddPizzaFlavourRequest()
        {
            
        }

        /// <summary>
        /// Creates a new instance of <see cref="AddPizzaFlavourRequest" />
        /// </summary>
        /// <param name="flavourName"></param>
        /// <param name="ingredients"></param>
        public AddPizzaFlavourRequest(string flavourName, IEnumerable<string> ingredients)
        {
            this.FlavourName = flavourName;
            this.Ingredients = new List<string>(ingredients);
        }
    }
}
