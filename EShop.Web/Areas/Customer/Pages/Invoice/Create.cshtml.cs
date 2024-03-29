using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using EShop.Data.Interfaces;
using EShop.Web.Areas.Catalog.Pages.Promotion;
using EShop.Web.Authorization;
using EShop.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Web.Areas.Customer.Pages.Invoice
{
    [Authorize(Policies.IsCustomer)]
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateModel(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [BindProperty]
        public List<CartItemVM> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }

        public void OnGet()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartItems = _unitOfWork.CartItemRepository.Get(x => x.UserId == userId, includeProperties: "Product,Product.ProductPromotions.Promotion").ToList();
            CartItems = _mapper.Map<List<CartItemVM>>(cartItems);
            DateTime now = DateTime.Now.Date;
            foreach (var item in CartItems)
            {
                var dbPromotion = cartItems.First(x => x.ProductId == item.ProductId).Product
                    .ProductPromotions.FirstOrDefault(x => x.Promotion.StartDate.Date <= now && x.Promotion.EndDate >= now)?.Promotion;
                item.Product.Promotion = _mapper.Map<PromotionVM>(dbPromotion);
            }

            TotalAmount = CartItems.Sum(x => x.Quantity * x.Product.Price);
            TotalDiscount = CartItems.Sum(x => x.Product.Discount);
        }

        public IActionResult OnPost()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartItems = _unitOfWork.CartItemRepository.Get(x => x.UserId == userId, includeProperties: "Product,Product.ProductPromotions.Promotion").ToList();
            
            CartItems = _mapper.Map<List<CartItemVM>>(cartItems);
            DateTime now = DateTime.Now.Date;
            foreach (var item in CartItems)
            {
                var dbPromotion = cartItems.First(x => x.ProductId == item.ProductId).Product
                    .ProductPromotions.FirstOrDefault(x => x.Promotion.StartDate.Date <= now && x.Promotion.EndDate >= now)?.Promotion;
                item.Product.Promotion = _mapper.Map<PromotionVM>(dbPromotion);
            }

            TotalAmount = CartItems.Sum(x => x.Quantity * x.Product.Price);
            TotalDiscount = CartItems.Sum(x => x.Product.Discount);
            
            var invoice = new Data.Entities.Invoice()
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = null,
                UserId = userId
            };

            invoice.InvoiceDetails = CartItems.Select(x => new Data.Entities.InvoiceDetail
            {
                CreatedDate = now,
                ModifiedDate = null,
                Invoice = invoice,
                ProductId = x.ProductId,
                PromotionId = x.Product.Promotion?.Id,
                UnitPrice = x.Product.Price,
                PromotionDiscount = x.Product.Discount,
                Quantity = x.Quantity
            }).ToList();

            _unitOfWork.InvoiceRepository.Insert(invoice);
            
            cartItems = _unitOfWork.CartItemRepository.Get(x => x.UserId == userId).ToList();
            foreach (var item in cartItems)
            {
                _unitOfWork.CartItemRepository.Delete(item);
            }
            _unitOfWork.Save();

            return RedirectToPage("./Index");
        }
    }
}
