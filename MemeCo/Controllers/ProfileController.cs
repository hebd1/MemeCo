/**
 * Author:    Jasen Lassig
 * Partner:   Jose Monterroso, Eli Hebdon
 * Date:      December 6, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jasen, Jose, Eli - This work may not be copied for use in Academic Coursework.
 *
 * I, Jasen, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *   Profile controller class, shows user profile
 */

using System.Linq;
using System.Threading.Tasks;
using MemeCo.Areas.Identity.Data;
using MemeCo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MemeCo.Controllers
{
    /// <summary>
    /// Profile controller class
    /// </summary>
    public class ProfileController : Controller
    {
        private MemeCoContext _context;
        private UserManager<MemeCoUser> _userManager;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public ProfileController(MemeCoContext context, UserManager<MemeCoUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays the users posts, followers, who they are following, likes, dislikes and posts
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("/{username}")]
        public async Task<IActionResult> Index(string username)
        {
            MemeCoUser user = await _userManager.FindByNameAsync(username);

            // Null check
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

        /// <summary>
        /// Follow a user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="follower"></param>
        /// <returns></returns>
        [HttpPost("/Profile/Follow")]
        public JsonResult Follow(string username, string follower)
        {
            // Get User
            var user =  _userManager.FindByNameAsync(username).Result;
            var follow = _userManager.FindByNameAsync(follower).Result;

            // Null check
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

        /// <summary>
        /// Unfollow a user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="follower"></param>
        /// <returns></returns>
        [HttpPost("/Profile/UnFollow")]
        public JsonResult UnFollow(string username, string follower)
        {
            // Get user
            var user = _userManager.FindByNameAsync(username).Result;
            var follow = _userManager.FindByNameAsync(follower).Result;

            // Null check
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