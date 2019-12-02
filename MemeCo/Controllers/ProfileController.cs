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
            MemeCoUser user = await _userManager.FindByNameAsync(username);

            user.Followers = _context.Follows.Where(x => x.UserID == user.Id).OrderBy(x => x.Follower.UserName).ToList();
            user.Following = _context.Follows.Where(x => x.FollowerID == user.Id).OrderBy(x => x.User.UserName).ToList();
            user.Likes = _context.Likes.Where(x => x.MemeCoUserID == user.Id).OrderBy(x => x.TimeLiked.Millisecond).ToList();
            user.Posts = _context.Posts.Where(x => x.MemeCoUserID == user.Id).OrderBy(x => x.TimePosted.Millisecond).ToList();
            user.Comments = _context.Comments.Where(x => x.MemeCoUserID == user.Id).OrderBy(x => x.TimeCommented.Millisecond).ToList();

            return View(user);
        }
    }
}