using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interface;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producers;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _eventBus;

        public BasketController(IBasketRepository basketRepository, IMapper mapper, EventBusRabbitMQProducer eventBus)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasketCart(string userName)
        {
            var result = await _basketRepository.GetBasketCart(userName);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasketCart(BasketCart basketCart)
        {
            var result = await _basketRepository.UpdateBasketCart(basketCart);
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteBasketCart(string userName)
        {
            var result = await _basketRepository.DeleteBasket(userName);
            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = _basketRepository.GetBasketCart(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            var eventBasketCheckout = _mapper.Map<BasketCheckoutEvent>(basketCheckout);

            eventBasketCheckout.RequestId = Guid.NewGuid();
            eventBasketCheckout.TotalPrice = basketCheckout.TotalPrice;

            try
            {
                _eventBus.PublishBaskCheckout(EventBusConstants.BasketCheckoutQueueName, eventBasketCheckout);
            }
            catch (Exception ex)
            {
                throw;
            }

            return Accepted();
        }
    }
}
