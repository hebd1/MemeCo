﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MemeCo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MemeCo.Areas.Identity.Data;

namespace MemeCo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MemeCoContext _context;
        private readonly UserManager<MemeCoUser> _user_manager;

        public HomeController(ILogger<HomeController> logger, MemeCoContext context, UserManager<MemeCoUser> um)
        {
            _logger = logger;
            _context = context;
            _user_manager = um;

        }

        public async Task<IActionResult> Index()
        {
            var result = _context.Posts.Include(o => o.Likes).Include(o => o.User).ThenInclude(o => o.Followers).Include(o => o.Comments).ToList();  
            return View(result);
        }


        [HttpPost]
        public async Task<JsonResult> Select_Filter(string user_id, string filter)
        {
            try
            {
                var user = await _user_manager.FindByIdAsync(user_id);
                // not logged in
                if (user == null)
                {
                    return Json(
                   new
                   {
                       success = false,
                       user_id = user_id,
                       filter = filter

                   });
                }
                else
                {
                    user.Filter = filter;
                    await _user_manager.UpdateAsync(user);

                    // success
                    return Json(
                        new
                        {
                            success = true,
                            user_id = user_id,
                            filter = filter
                        });
                }
                // something else went wrong
            }
            catch (Exception)
            {
                return Json(
                   new
                   {
                       success = false,
                       user_id = user_id,
                       filter = filter
                   });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Like_Post(string user_id, bool liked, Guid post_id)
        {
            try
            {
                Post post = _context.Posts.Where(o => o.ID == post_id).FirstOrDefault();
                var user = await _user_manager.FindByIdAsync(user_id);
                // not logged in
                if (user == null)
                {
                    return Json(
                   new
                   {
                       success = false,
                       user_id = user_id,
                       liked = liked,
                       post_id = post_id,

                   });
                }
                // get previous reaction of same type from user if it exists
                if (_context.Likes.Where(o => o.Post == post).Any(o => o.MemeCoUserID == user.Id && o.Liked == liked))
                {
                   
                    Like reaction = _context.Likes.Where(o => o.MemeCoUserID == user.Id && o.Post == post && o.Liked == liked).First();
                    // undo like/dislike
                    _context.Likes.Remove(reaction);
                   
                }
                else
                {
                    // user had a previous reaction that was the opposite of the current reaction
                    if (_context.Likes.Where(o => o.Post == post).Any(o => o.MemeCoUserID == user.Id && o.Liked == !liked))
                    {
                        Like previousReaction = _context.Likes.Where(o => o.MemeCoUserID == user.Id && o.Post == post && o.Liked == !liked).First();
                        _context.Likes.Remove(previousReaction);


                    }
                    Like like = new Like();
                    like.Liked = liked;
                    like.MemeCoUserID = user.Id;
                    like.Post = post;
                    like.User = user;
                    await _context.Likes.AddAsync(like);
                }
               
                _context.SaveChanges();

                // calculate new like/dislike ratio
                double likes = _context.Likes.Where(o => o.Liked == true && o.Post == post).Count();
                double dislikes = _context.Likes.Where(o => o.Liked == false && o.Post == post).Count();
                var likePercent = "";
                var dislikePercent = "";
                if (likes > 0)
                {
                    likePercent = Math.Ceiling(likes / (likes + dislikes) * 100) + "%";
                }
                if (dislikes > 0)
                {
                    dislikePercent = Math.Ceiling(dislikes / (likes + dislikes) * 100) + "%";
                }

                // success
                return Json(
                    new
                    {
                        success = true,
                        user_id = user_id,
                        liked = liked,
                        post_id = post_id,
                        like_percent = likePercent,
                        dislike_percent = dislikePercent
                    });
                // something else went wrong
            } catch (Exception)
            {
                return Json(
                   new
                   {
                       success = false,
                       user_id = user_id,
                       liked = liked,
                       post_id = post_id,
                       like_percent = 0,
                       dislike_percent = 0
                   });
            }
           
        }

        /// <summary>
        /// Saves the Theme chosen by the user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Set_Theme(string userid, string theme)
        {
            try
            {
                // Assign the theme
                bool userTheme;
                if (theme.Equals("light"))
                {
                    userTheme = true;
                }
                else
                {
                   userTheme = false;
                }

                // Get user theme and change it 
                var user = await _user_manager.FindByIdAsync(userid);
                user.DarkMode = userTheme; 
                _context.SaveChanges();

                // Return user theme
                return Json(new {    
                    success = true,    
                });
            }
            catch(Exception)
            {
                // Any issues 
                return Json(new{
                    success = false,   
                });
            }
        }

        /// <summary>
        /// Get the perfered theme of the user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Get_Theme(string username, string password, bool darkmode)
        {
            try
            {
                // To confirm accurate user
                PasswordHasher<MemeCoUser> ph = new PasswordHasher<MemeCoUser>();

                // Get user theme
                MemeCoUser user = await _user_manager.FindByNameAsync(username);

                // Verify it's the correct user
                if(ph.VerifyHashedPassword(user, user.PasswordHash, password).Equals(PasswordVerificationResult.Success))
                {
                    bool dM = user.DarkMode;
                    // Return user theme
                    return Json(new
                    {
                        success = true,
                        username = username,
                        password = password,
                        darkmode = dM
                    });
                }
                else
                {
                    // Any issues 
                    return Json(new
                    {
                        success = false,
                        username = username,
                        password = password,
                        darkmode = darkmode
                    });
                }
            }
            catch (Exception)
            {
                // Any issues 
                return Json(new
                {
                    success = false,
                    username = username,
                    password = password,
                    darkmode = darkmode
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}