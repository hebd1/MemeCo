using System;
using System.Collections.Generic;
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
            var result = _context.Posts.Include(o => o.Likes).Include(o => o.User).Include(o => o.Comments).ToList();  
            return View(result);
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
                // get previous reaction from user if it exists
                if (_context.Likes.Where(o => o.Post == post).Any(o => o.MemeCoUserID == user.Id))
                {
                    Like like = _context.Likes.Where(o => o.MemeCoUserID == user.Id && o.Post == post).First();
                    like.Liked = liked;
                    _context.Likes.Update(like);
                } else
                {
                    Like like = new Like();
                    like.Liked = liked;
                    like.MemeCoUserID = user.Id;
                    like.Post = post;
                    like.User = user;
                    await _context.Likes.AddAsync(like);
                }
               
                _context.SaveChanges();

                // success
                return Json(
                    new
                    {
                        success = true,
                        user_id = user_id,
                        liked = liked,
                        post_id = post_id,
                    });
                // something else went wrong
            } catch (Exception e)
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
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
