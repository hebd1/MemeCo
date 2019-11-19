using System;
using System.Threading.Tasks;
using MemeCo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemeCo.Controllers
{
    public class IndividualMemeController : Controller
    {
        private MemeCoContext _context;

        public IndividualMemeController(MemeCoContext context)
        {
            _context = context;
        }

        [HttpGet("/{postID}")]
        public async Task<IActionResult> Index(Guid postID)
        {
            var post = await _context.Posts.Include(o => o.Comments).ThenInclude(i => i.User).FirstOrDefaultAsync(p => p.ID == postID);
            return View(post);
        }
    }
}