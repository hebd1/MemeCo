using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            if (user == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }

            user.Followers = _context.Follows.Where(x => x.UserID == user.Id).OrderBy(x => x.Follower.UserName).ToList();
            user.Following = _context.Follows.Where(x => x.FollowerID == user.Id).OrderBy(x => x.User.UserName).ToList();
            user.Likes = _context.Likes.Where(x => x.MemeCoUserID == user.Id).OrderBy(x => x.TimeLiked.Millisecond).ToList();
            user.Posts = _context.Posts.Where(x => x.MemeCoUserID == user.Id).OrderBy(x => x.TimePosted.Millisecond).ToList();
            user.Comments = _context.Comments.Where(x => x.MemeCoUserID == user.Id).OrderBy(x => x.TimeCommented.Millisecond).ToList();

            return View(user);
        }
        [HttpPost("/Profile/Follow")]
        public IActionResult Follow(string username, string follower)
        {
            var user =  _userManager.FindByNameAsync(username).Result;
            var follow = _userManager.FindByNameAsync(follower).Result;

            if (user == null || follow == null)
            {
                return Json(new { success = false });
            }

            var temp = new Follow();
            temp.UserID = user.Id;
            temp.FollowerID = follow.Id;
            temp.User = user;
            temp.Follower = follow;

            var result = _context.Follows.Add(temp);
            _context.SaveChanges();

            return Json(new { success = true, followNum = _context.Follows.Count(x => x.UserID == user.Id) });
        }
        [HttpPost("/Profile/UnFollow")]
        public IActionResult UnFollow(string username, string follower)
        {
            var user = _userManager.FindByNameAsync(username).Result;
            var follow = _userManager.FindByNameAsync(follower).Result;

            if (username == follower)
            {
                return Json(new { success = true, followNum = _context.Follows.Count(x => x.UserID == user.Id) });
            }
            if (user == null || follow == null)
            {
                return Json(new { success = false });
            }

            var temp = new Follow();
            temp.UserID = user.Id;
            temp.FollowerID = follow.Id;
            temp.User = user;
            temp.Follower = follow;

            _context.Follows.Remove(temp);
            _context.SaveChanges();

            return Json(new { success = true, followNum = _context.Follows.Count(x => x.UserID == user.Id) });
        }
    }
}