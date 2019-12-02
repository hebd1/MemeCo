using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MemeCo.Areas.Identity.Data;
using MemeCo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace MemeCo.Controllers
{
    public class EditorController : Controller
    {
        private readonly MemeCoContext _context;
        private readonly UserManager<MemeCoUser> _user_manager;


        public EditorController(MemeCoContext context, UserManager<MemeCoUser> user_manager)
        {
            _context = context;
            _user_manager = user_manager;
        }

        [HttpGet("/editor")]
        public IActionResult Index()
        {
            var result = _context.Posts.Include(o => o.Likes).Include(o => o.User).ThenInclude(o => o.Followers).Include(o => o.Comments).ToList();
            return View(result);
        }

        [HttpPost]
        public async Task<JsonResult> Post_Meme(string user_id, string description, string image, int template_id)
        {
          

            try
            {
                var user = await _user_manager.FindByIdAsync(user_id);
                image = image.Substring(image.LastIndexOf(',') + 1);
                byte[] img = Convert.FromBase64String(image);
                // save image array to DB
                if (description == null)
                {
                    description = "";
                }
                Post post = new Post();
                post.Description = description;
                post.Meme = img;
                post.MemeCoUserID = user.Id;
                post.User = user;
                _context.Posts.Add(post);
                _context.SaveChanges();


                // not logged in
                if (user == null)
                {
                    return Json(
                   new
                   {
                       success = false,
                       user_id = user_id,
                       description = description,
                       image = image,
                       template_id = template_id

                   });
                }
                else
                {
                   

                    // success
                    return Json(
                        new
                        {
                            success = true,
                            user_id = user_id,
                            description = description,
                            image = image,
                            template_id = template_id

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
                       description = description,
                       image = image,
                       template_id = template_id

                   });
            }
        }
    }
}