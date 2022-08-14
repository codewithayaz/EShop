using AutoMapper;
using Microsoft.AspNetCore.Identity;
using EShop.Data.Entities;
using EShop.Web.Areas.Admin.Pages.Role;
using EShop.Web.Areas.Admin.Pages.User;
using EShop.Web.Areas.Catalog.Pages.Category;
using EShop.Web.Areas.Catalog.Pages.Product;
using EShop.Web.Areas.Catalog.Pages.Promotion;
using EShop.Web.Models;
using EShop.Web.Areas.Customer.Pages.Invoice;

namespace EShop.Web.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<string, string>().ConvertUsing(s => s ?? string.Empty);


            CreateMap<IdentityRole, RoleVM>().ReverseMap();
            CreateMap<ApplicationUser, UserVM>().ReverseMap();
            CreateMap<Category, CategoryVM>().ReverseMap();
            CreateMap<Product, ProductVM>().ReverseMap();
            CreateMap<Promotion, PromotionVM>().ReverseMap();
            CreateMap<CartItem, CartItemVM>().ReverseMap();
            CreateMap<Invoice, InvoiceVM>().ReverseMap();
            CreateMap<InvoiceDetail, InvoiceDetailVM>().ReverseMap();

        }
    }
}
