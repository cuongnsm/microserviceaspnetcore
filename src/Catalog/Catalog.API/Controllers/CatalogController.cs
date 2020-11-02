using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepositories _productRepositories;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepositories productRepositories, ILogger<CatalogController> logger)
        {
            _productRepositories = productRepositories;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _productRepositories.GetProducts();

            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetProduct(string id)
        {
            var product = await _productRepositories.GetProduct(id);

            if (product == null)
            {
                _logger.LogError($"Product id: {id}, not found");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        [Route("[action]/{name}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GeProductsByName(string name)
        {
            var products = await _productRepositories.GetProductsByName(name);

            return Ok(products);
        }

        [HttpGet]
        [Route("[action]/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetProductsByCategory(string category)
        {
            var products = await _productRepositories.GetProductsByCatagory(category);

            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateProduct([FromBody] Product product)
        {
            await _productRepositories.Create(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product );
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpateProduct([FromBody] Product product)
        {
            return Ok(await _productRepositories.Update(product));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            return Ok(await _productRepositories.Delete(id));
        }
    }
}
