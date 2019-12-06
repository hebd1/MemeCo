/**
 * Author:    Jasen Lassig
 * Partner:   Jose Monterroso, Eli Hebdon
 * Date:      December 6, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jasen, Jose, Eli - This work may not be copied for use in Academic Coursework.
 *
 * I, Jassen, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    Post controller class, used to display post and comments
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using MemeCo.Areas.Identity.Data;
using MemeCo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemeCo.Controllers
{
    /// <summary>
    /// Post Controller class
    /// </summary>
    public class PostController : Controller
    {
        private MemeCoContext _context;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="context"></param>
        public PostController(MemeCoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Individual post display
        /// </summary>
        /// <param name="postID"></param>
        /// <returns></returns>
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

            if (post == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }

            return View(post);
        }

        /// <summary>
        /// Adds a new user comment to a post
        /// </summary>
        /// <param name="post_id"></param>
        /// <param name="user_id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<JsonResult> add_comment(Guid post_id, string user_id, string comment)
        {
            try
            {
                // Ensure comment has content
                if(comment == null)
                {
                    return Json(new
                    {
                        success = false
                    });
                }
                // Find user and post
                MemeCoUser user = await _context.Users.Where(u => u.Id == user_id).FirstAsync();
                Post post = await _context.Posts.Where(p => p.ID == post_id).FirstAsync();

                // Create comment
                Comment c = new Comment
                {
                    Content = comment,
                    Post = post,
                    User = user,
                    TimeCommented = DateTime.UtcNow
            };

                // Save changes
                _context.Comments.Add(c);
                _context.SaveChanges();

                var uC = await _context.Comments.Where(c => c.Content == comment && c.Post == post && c.User == user).FirstAsync();
                string profilePic = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(uC.User.ProfilePicture));

                return Json(new
                {
                    success = true,
                    username = user.UserName,
                    profilepic = profilePic,
                    commentid = uC.ID,
                    description = comment,
                    userid = user_id,
                    postid = post.ID,
                    time = uC.TimeCommented
                });
            }
            catch (Exception)
            {
                // Error
                return Json(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// Deletes the user comment
        /// </summary>
        /// <param name="comment_id"></param>
        /// <returns></returns>
        public async Task<JsonResult> delete_comment(int comment_id)
        {
            try
            {
                // Find Comment to delete
                Comment comment = await _context.Comments.Where(i => i.ID == comment_id).FirstAsync();
                _context.Comments.Remove(comment);
                _context.SaveChanges();

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// Edit the user comment
        /// </summary>
        /// <param name="comment_id"></param>
        /// <param name="comment_text"></param>
        /// <returns></returns>
        public async Task<JsonResult> edit_comment(int comment_id, string comment_text)
        {
            try
            {

                // editing comment
                Comment comment = await _context.Comments.Where(c => c.ID == comment_id).FirstAsync();
                string oldComment = comment.Content;

                // Checking if comment contains nothing
                if (comment_text == null)
                {
                    return Json(new
                    {
                        success = true,
                        isnull = true,
                        comment_text = oldComment
                    });
                }
                else
                {
                    comment.Content = comment_text;
                    comment.TimeCommented = DateTime.UtcNow;
                    _context.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        isnull = false
                    });
                }
            }
            catch (Exception)
            {
                // Any issues finding/editing comment
                return Json(new
                {
                    success = false
                });
            }
        }
    }
}