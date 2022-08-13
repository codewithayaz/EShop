using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace EShop.Web.Pages
{
    [AllowAnonymous]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int? code = null)
        {
            //var statusCodeResult =
            //    HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (code)
            {
                case 404:
                    ViewData["ErrorMessage"] = "Sorry, the resource you requested could not be found";
                    break;
                case 401:
                    ViewData["ErrorMessage"] = "Authorization required.";
                    break;
                case 403:
                    ViewData["ErrorMessage"] = "Forbidden";
                    break;
                case 500:
                    ViewData["ErrorMessage"] = "Internal server error";
                    break;

                default:
                    break;
            }

            
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            
        }
    }
}