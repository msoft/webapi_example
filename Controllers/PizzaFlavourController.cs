using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApiExample.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApiExample.Controllers
{
    /// <summary>
    /// Controller used for fetching information about Pizza Flavour.
    /// <summary>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Get or create new flavour for pizzas")]
    public class PizzaFlavourController : ControllerBase
    {
        private readonly IPizzaFlavourRepositoryService flavourService;

        public PizzaFlavourController(IPizzaFlavourRepositoryService flavourService)
        {
            this.flavourService = flavourService;
        }

  
        [HttpGet]
        [SwaggerResponse(201, "The product was created", typeof(IEnumerable<string>))]    
        public ActionResult<IEnumerable<string>> GetFlavourNames()
        {
            return Ok(this.flavourService.GetFlavours().Select(f => f.Value));
        }

        
        // /// <summary>
        // /// Find flavour using flavour name
        // /// </summary>
        // /// <remarks>Usefull remark</remarks>
        // /// <response code="200">Flavour retreived</response>
        // /// <response code="400">Flavour not found</response>
        // /// <response code="500">Bad request</response>
        // [HttpGet("{flavourName}", Name = "FindFlavourByName")]
        // [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        // [ProducesResponseType(typeof(string), 400)]
        // [ProducesResponseType(500)]
        // public ActionResult<IEnumerable<string>> FindFlavour(string flavourName)
        // {
        //     if (string.IsNullOrEmpty(flavourName))
        //     {
        //         return BadRequest($"The parameter '{nameof(flavourName)}' shall be provided.");
        //     }

        //     if (!this.flavourService.TryGetIngredients(flavourName, out IEnumerable<string> ingredients))
        //     {
        //         return NotFound();
        //     }

        //     return Ok(ingredients);
        // }


        // [HttpGet("{flavourName}", Name = "FindFlavourByName")]
        // // [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        // // [ProducesResponseType(typeof(string), 400)]
        // // [ProducesResponseType(500)]
        // public ActionResult<IEnumerable<string>> FindFlavour([FromQuery, BindRequired]string flavourName)
        // {
        //     if (string.IsNullOrEmpty(flavourName))
        //     {
        //         return BadRequest($"The parameter '{nameof(flavourName)}' shall be provided.");
        //     }

        //     if (!this.flavourService.TryGetIngredients(flavourName, out IEnumerable<string> ingredients))
        //     {
        //         return NotFound();
        //     }

        //     return Ok(ingredients);
        // }


        [HttpGet("{flavourName}", Name = "FindFlavourByName")]
        [SwaggerOperation(
                Summary = "Returns the ingredients from a flavour name",
                Description = "Returns the ingredients",
                OperationId = "FindFlavour",
                Tags = new[] { "Flavour", "Pizza" }
            )]
        [SwaggerResponse(200, "The ingredients for the flavour have been found", typeof(IEnumerable<string>))]
        [SwaggerResponse(400, "The flavour has not been found", typeof(string))]
        [SwaggerResponse(500, "Internal server error")]
        public ActionResult<IEnumerable<string>> FindFlavour(
            [FromQuery, SwaggerParameter("Flavour name", Required = true)]string flavourName)
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
