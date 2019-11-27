/**
 * Author:    Jose Monterroso
 * Partner:   Jasen Lassig, Eli Hebdon
 * Date:      November 27, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jose, Jasen, Eli - This work may not be copied for use in Academic Coursework.
 *
 * I, Jose, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    Returns the related posts based on the template ID, otherwise we return random Records
 */

using MemeCo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeCo.ViewComponents
{
    /// <summary>
    /// Related ViewComponent class. Allows us to find the 
    /// related posts from the template ID. A View component
    /// can be used multiple times throughout various pages
    /// </summary>
    [ViewComponent(Name = "Related")]
    public class RelatedViewComponent : ViewComponent
    {
        private MemeCoContext _context;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="context"></param>
        public RelatedViewComponent(MemeCoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Given a template ID we return the related Posts
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="post"></param>
        /// <param name="isEditor"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int templateID, Post post, bool isEditor)
        {
            List<Post> relatedPosts;

            // isEditor vs normal page
            if (isEditor)
            {
                // Null Check
                if (templateID == -1)
                {
                    // Random Posts
                    relatedPosts = await _context.Posts
                            .Include(u => u.User)
                            .OrderBy(r => Guid.NewGuid()).Skip(3).Take(10).ToListAsync();
                }
                else
                {
                    // Related Posts
                    relatedPosts = await _context.Posts
                        .OrderBy(r => Guid.NewGuid())
                        .Include(u => u.User)
                        .Where(p => p.TemplateID == templateID)
                        .Skip(3).Take(10).ToListAsync();

                    // Fills the remaining count of the related memes
                    if (relatedPosts.Count < 10)
                    {
                        // Gets random Posts
                        List<Post> randomPosts = await _context.Posts
                            .Include(u => u.User)
                            .OrderBy(r => Guid.NewGuid()).Skip(3).Take(10 - relatedPosts.Count).ToListAsync();

                        // Adds to the related Posts
                        relatedPosts.AddRange(randomPosts);
                    }
                }
            }
            else
            {
                // Null Check
                if (templateID == -1)
                {
                    // Random Posts
                    relatedPosts = await _context.Posts
                            .Include(u => u.User)
                            .Where(p => p.ID != post.ID)
                            .OrderBy(r => Guid.NewGuid()).Skip(3).Take(10).ToListAsync();
                }
                else
                {
                    // Related Posts
                    relatedPosts = await _context.Posts
                        .OrderBy(r => Guid.NewGuid())
                        .Include(u => u.User)
                        .Where(p => p.TemplateID == templateID && p.ID != post.ID)
                        .Skip(3).Take(10).ToListAsync();

                    // Fills the remaining count of the related memes
                    if (relatedPosts.Count < 10)
                    {
                        // Gets random Posts
                        List<Post> randomPosts = await _context.Posts
                            .Include(u => u.User)
                            .Where(p => p.ID != post.ID)
                            .OrderBy(r => Guid.NewGuid()).Skip(3).Take(10 - relatedPosts.Count).ToListAsync();

                        // Adds to the related Posts
                        relatedPosts.AddRange(randomPosts);
                    }
                }
            }

            // Giving the View the related Posts
            return View(relatedPosts);
        }
    }
}