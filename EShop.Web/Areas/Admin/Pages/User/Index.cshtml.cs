using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using EShop.Core.Extensions;
using EShop.Core.Interfaces;
using EShop.Core.Models;
using EShop.Data.Entities;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Admin.Pages.User
{
    [Authorize(Permissions.User.Read)]
    public class UserModel : PageModel
    {
        private readonly EShop.Data.ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserModel(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, IServiceProvider serviceProvider)
        {
            _roleManager = roleManager;
            _userManager = userManager;

            _mapper = serviceProvider.GetRequiredService<IMapper>();
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
        public PaginatedResult<UserVM> PaginatedResult { get; set; }

        public IEnumerable<SelectListItem> PageSizeList { get; set; } = new SelectList(new List<int> { 5, 10, 25, 50, 100 });

        public void OnGet([FromQuery] int page = 1)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.SearchColumnList = new Dictionary<string, string>();
            filter.SearchColumnList.Add("Email", "string");
            filter.SearchColumnList.Add("FirstName", "string");
            filter.SearchColumnList.Add("LastName", "string");
            filter.SearchColumnList.Add("PhoneNumber", "string");

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

            PaginatedResult = _userManager.Users.AsNoTracking().GetPaginatedResults<UserVM>(filter, _mapper);
        }
    }
}
