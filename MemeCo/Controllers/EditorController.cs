/**
 * Author:    Eli Hebdon
 * Partner:   Jasen Lassig, Jose Monterroso
 * Date:      December 3, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jose, Jasen, Eli - This work may not be copied for use in Academic Coursework.
 *
 * I, Eli, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    Controller for meme editor. Handles posting memes and getting related memes.
 */
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
    /// <summary>
    /// Editor Controller class
    /// </summary>
    public class EditorController : Controller
    {
        private readonly MemeCoContext _context;
        private readonly UserManager<MemeCoUser> _user_manager;

        /// <summary>
        /// Controller Constructor. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user_manager"></param>
        public EditorController(MemeCoContext context, UserManager<MemeCoUser> user_manager)
        {
            _context = context;
            _user_manager = user_manager;
        }

        /// <summary>
        /// Returns main view of the meme editor containing all meme templates
        /// </summary>
        /// <returns></returns>
        [HttpGet("/editor")]
        public IActionResult Index()
        {
            if(this.User.Identity.Name != null)
            {
               var result = _context.Templates;
                return View(result);
            }
            // user not logged in
            else
            {
                return Redirect("Identity/Account/Login");
            }
            
        }

        /// <summary>
        /// Helper function that returns the view component containing templates with the input template ID.
        /// </summary>
        /// <param name="_templateID"></param>
        /// <returns></returns>
        public IActionResult GetComponent(int _templateID)
        {
            return ViewComponent("Related", new { templateID = _templateID, post = new Post(), isEditor = true });
        }

        /// <summary>
        /// Adds a post to the database and returns a Json result. Result is unsuccessful if user is not logged in.
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="description"></param>
        /// <param name="image"></param>
        /// <param name="template_id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Post_Meme(string user_id, string description, string image, int template_id)
        {
            try
            {
                var user = await _user_manager.FindByIdAsync(user_id);
                image = image.Substring(image.LastIndexOf(',') + 1);
                byte[] img = Convert.FromBase64String(image);

                if (description == null)
                {
                    description = "";
                }

                // Post to post
                Post post = new Post();
                post.Description = description;
                post.TemplateID = template_id;
                post.Meme = img;
                post.MemeCoUserID = user.Id;
                post.User = user;
                post.TemplateID = template_id;
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
            }
            // something else went wrong
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