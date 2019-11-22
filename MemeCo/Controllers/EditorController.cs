using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MemeCo.Areas.Identity.Data;
using MemeCo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


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
                var img = System.IO.File.ReadAllBytes(image);

                //byte[] imageArr;
                //if (image != null)
                //{
                //    using (var ms = new MemoryStream())
                //    {
                //        image.CopyTo(ms);
                //        imageArr = ms.ToArray();
                //    }
                //}

                // save image array to DB


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
                       
                   });
            }
        }
    }
}