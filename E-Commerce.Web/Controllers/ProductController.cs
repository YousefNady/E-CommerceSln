using E_Commerce.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // Get: BaseURL/api/Product/10 
        // No Need To Add EndPoint Name(Action)
        // URL & Methodبيعرف الاند بوينت عن طريق ال 
        // HttpGetهيدور في الكنترول علي اند بوينت بتاخد ميسود 

        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            return new Product { Id = id, Name = "Test" };
        
        }

        // Get: BaseURL/api/Product
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return new List<Product>();
        }
    }
}
