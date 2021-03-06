﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheSongList.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet, Authorize(Policy = "CanEdit")]
        public IActionResult Login(string returnUrl) => Redirect(returnUrl);

        [HttpGet]
        public IActionResult Logout() =>
            Redirect($"https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout");

        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}