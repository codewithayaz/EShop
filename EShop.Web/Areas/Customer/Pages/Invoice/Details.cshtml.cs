using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using EShop.Data.Interfaces;
using EShop.Web.Areas.Catalog.Pages.Promotion;
using EShop.Web.Authorization;
using EShop.Web.Models;

namespace EShop.Web.Areas.Customer.Pages.Invoice
{
    [Authorize(Policies.IsCustomer)]
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DetailsModel(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [BindProperty]
        public InvoiceVM Entity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }

        public void OnGet(int id)
        {
            var invoice = _unitOfWork.InvoiceRepository.Get(x => x.Id == id, includeProperties: "InvoiceDetails.Product,InvoiceDetails.Promotion").FirstOrDefault();
            Entity = _mapper.Map<InvoiceVM>(invoice);
            foreach (var item in Entity.InvoiceDetails)
            {
                item.Product.Promotion = item.Promotion;
            }
            DateTime now = DateTime.Now.Date;
            
            TotalAmount = Entity.InvoiceDetails.Sum(x => x.Quantity * x.Product.Price);
            TotalDiscount = Entity.InvoiceDetails.Sum(x => x.Product.Discount);
        }

    }
}
