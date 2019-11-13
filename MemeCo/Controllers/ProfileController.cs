using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeCo.Areas.Identity.Data;
using MemeCo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MemeCo.Controllers
{
    public class ProfileController : Controller
    {
        private MemeCoContext _context;
        private UserManager<MemeCoUser> _userManager;
        public ProfileController(MemeCoContext context, UserManager<MemeCoUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet("/{username}")]
        public async Task<IActionResult> Index(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            return View(user);
        }
    }
}