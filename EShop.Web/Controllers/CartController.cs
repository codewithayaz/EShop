using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EShop.Data.Interfaces;
using EShop.Web.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policies.IsCustomer)]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("getitems")]
        public void Get()
        {

        }

        [HttpPost("additem")]
        public ActionResult Post([FromBody] int productId)
        {
            var product = _unitOfWork.ProductRepository.GetByID(productId);
            if (product == null)
            {
                return NotFound($"Product not found.");
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var dbEntity = _unitOfWork.CartItemRepository.Get(x => x.UserId == userId && x.ProductId == productId).FirstOrDefault();
            if (dbEntity != null)
            {
                dbEntity.Quantity++;
                _unitOfWork.CartItemRepository.Update(dbEntity);
            }
            else
            {
                _unitOfWork.CartItemRepository.Insert(new Data.Entities.CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = 1,
                    CreatedDate = DateTime.Now
                });
            }
            _unitOfWork.Save();

            return Ok("Product added to cart successfully.");
        }

        [HttpPost("removeitem")]
        public ActionResult RemoveCartItem([FromBody] int productId)
        {
            var product = _unitOfWork.ProductRepository.GetByID(productId);
            if (product == null)
            {
                return NotFound($"Product not found.");
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var dbEntity = _unitOfWork.CartItemRepository.Get(x => x.UserId == userId && x.ProductId == productId).First();
            _unitOfWork.CartItemRepository.Delete(dbEntity);
            _unitOfWork.Save();

            return Ok("Product removed from cart.");
        }

    }
}
