﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using EShop.Core.Interfaces;
using EShop.Core.Models;
using EShop.Data.Interfaces;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Catalog.Pages.Promotion
{
    [Authorize(Permissions.Promotion.Read)]
    public class ListModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public PaginatedResult<PromotionVM> PaginatedResult { get; set; }

        public IEnumerable<SelectListItem> PageSizeList { get; set; } = new SelectList(new List<int> { 5, 10, 25, 50, 100 });

        public void OnGet([FromQuery] int page = 1)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.SearchColumnList = new Dictionary<string, string>();
            filter.SearchColumnList.Add("Name", "string");
            filter.SearchColumnList.Add("Description", "string");
            filter.SearchColumnList.Add("DiscountPercent", "number");

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
            
            PaginatedResult = _unitOfWork.PromotionRepository.GetPaginatedResults<PromotionVM>(filter);
        }

    }
}
