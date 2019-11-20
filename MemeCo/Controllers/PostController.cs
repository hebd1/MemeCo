using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeCo.Areas.Identity.Data;
using MemeCo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemeCo.Controllers
{
    public class PostController : Controller
    {
        private MemeCoContext _context;
        public PostController(MemeCoContext context)
        {
            _context = context;
        }
        [HttpGet("Post/{postID}")]
        public IActionResult Index(Guid postID)
        {
            Post post = _context.Posts
                .Include(o => o.Likes)
                .Include(o => o.User)
                    .ThenInclude(o => o.Followers)
                .Include(o => o.Comments)
                    .ThenInclude(o => o.User)
                .SingleOrDefault(o => o.ID == postID);
            
            return View(post);
        }
    }
}