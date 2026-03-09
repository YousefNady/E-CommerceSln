using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;


namespace E_Commerce.Presentation.Controllers
{
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            _basketService = basketService;
        }


        // GET: BaseURL/api/Baskets?id={id} // id as query parameter (Query String)
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket(string id)
        {
            var Basket = await _basketService.GetBasketAsync(id);
            return Ok(Basket);
        }

        // POST: BaseURL/api/Baskets
        // will send BasketDTO in the request body
        [HttpPost]

        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basket)
        {
            var Basket = await _basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }


        // DELETE: BaseURL/api/Baskets/{id} [id => in Route]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var Result = await _basketService.DeleteBasketAsync(id);
            return Ok(Result);
        }

    }
}
