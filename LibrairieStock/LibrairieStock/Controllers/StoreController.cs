namespace LibrairieStock.Controllers
{
    using LibrairieStock.CustomExceptions;
    using LibrairieStock.Intarfeces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System;

    [Route("[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStore storeService;
        private IMemoryCache cache;

        public StoreController(IStore storeService, IMemoryCache cache)
        {
            this.storeService = storeService;
            this.cache = cache;
        }

        [HttpGet("getquantite/{name}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Quantity(string name)
        {
            try
            {
                var quantite = this.storeService.Quantity(name);
                return this.Ok(quantite);
            }
            catch (Exception ec)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("paniers")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CalculPrixPaniers([FromQuery]string[] basketByNames)
        {
            try
            {
                var result = this.storeService.Buy(basketByNames);
                return Ok(result);
            }
            catch (NotEnoughInventoryException ex)
            {
                return BadRequest(ex.Missing);
            }
            catch (Exception ec)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
