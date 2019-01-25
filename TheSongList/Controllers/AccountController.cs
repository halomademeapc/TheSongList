using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheSongList.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet, Authorize]
        public IActionResult Login(string returnUrl) => Redirect(returnUrl);

        [HttpGet]
        public IActionResult Logout()
        {
            var target = $"{Request.Scheme}://{Request.Host}";
            return Redirect($"https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout?continue={target}");
        }
    }
}