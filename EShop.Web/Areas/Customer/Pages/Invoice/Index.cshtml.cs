using AutoMapper;
using EShop.Core.Constants;
using EShop.Core.Extensions;
using EShop.Core.Models;
using EShop.Data.Interfaces;
using EShop.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EShop.Web.Areas.Customer.Pages.Invoice
{
    [Authorize(Policies.IsCustomer)]
    public class ListModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ListModel(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortField { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SortDir { get; set; }

        //for the next sort direction when the user clicks on the header
        [BindProperty(SupportsGet = true)]
        public int? SortDirNext { get; set; }
        [BindProperty(SupportsGet = true)]
        [MaxLength(15)]
        public string Search { get; set; }
        public PaginatedResult<InvoiceVM> PaginatedResult { get; set; }

        public IEnumerable<SelectListItem> PageSizeList { get; set; } = new SelectList(new List<int> { 5, 10, 25, 50, 100 });

        public void OnGet([FromQuery] int page = 1)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.SearchColumnList = new Dictionary<string, string>();
            filter.SearchColumnList.Add("CreatedDate", "date");
            filter.SearchColumnList.Add("User.Email", "string");

            if (PageSize.HasValue)
                filter.PageSize = (int)PageSize;

            filter.PageNumber = PageNumber;

            //if never sorted
            if (SortField == null)
                SortDir = 0;
            else if (SortDirNext != null)  //if requested new sort direction
                SortDir = SortDirNext.Value;

            //SortDirNext will be the reverse of SortDir
            SortDirNext = SortDir == 1 ? 2 : 1;

            filter.SortLabel = SortField;
            filter.SortDirection = SortDir;
            filter.Search = Search;
            if (User.IsInRole(DefaultRoles.Admin))
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var invoices = _unitOfWork.InvoiceRepository.Get(x=>x.UserId==userId, "User,InvoiceDetails.Product,InvoiceDetails.Promotion");
                PaginatedResult = invoices.GetPaginatedResults<InvoiceVM>(filter, _mapper);
            }
            else
            {
                PaginatedResult = _unitOfWork.InvoiceRepository.GetPaginatedResults<InvoiceVM>(filter, "User,InvoiceDetail.Product,InvoiceDetails.Promotion");
            }
        }

    }
}
