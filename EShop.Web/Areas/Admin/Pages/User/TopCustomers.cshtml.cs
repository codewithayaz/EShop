using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using EShop.Core.Constants;
using EShop.Core.Extensions;
using EShop.Core.Interfaces;
using EShop.Core.Models;
using EShop.Data.Entities;
using EShop.Data.Interfaces;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Admin.Pages.User
{
    [Authorize(Permissions.User.Read)]
    public class TopCustomersModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TopCustomersModel(IUnitOfWork unitOfWork, IMapper mapper, RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        [MaxLength(15)]
        public string Search { get; set; }
        [BindProperty(SupportsGet = true), Display(Name = "Start Date"), DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-30);
        [BindProperty(SupportsGet = true), Display(Name = "End Date"), DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Now;
        public List<UserVM> Customers { get; set; }

        public async Task OnGetAsync()
        {
            var audits = _unitOfWork.UserAuditRepository.Get(x => x.EventDate.Date >= StartDate.Date && x.EventDate.Date <= EndDate.Date)
                                                        .GroupBy(o => o.UserId)
                                                        .OrderByDescending(og => og.Count())
                                                        .Take(10).Select(x => x.FirstOrDefault()).ToList();

            var customers = await _userManager.GetUsersInRoleAsync(DefaultRoles.Customer);
            customers = customers.Where(c => audits.Select(x => x.UserId).Contains(c.Id)).ToList();
            if (!string.IsNullOrWhiteSpace(Search))
            {
                customers = customers.Where(x => x.FirstName.Contains(Search) || x.LastName.Contains(Search)
                || x.Email.Contains(Search)).ToList();
            }
            Customers = _mapper.Map<List<UserVM>>(customers);
        }
    }
}
