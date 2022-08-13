using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.Web.Pages
{
    [AllowAnonymous]
    public class ContactModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
