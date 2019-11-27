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

using MemeCo.Areas.Identity.Data;
using MemeCo.Models;
using Microsoft.AspNetCore.Identity;
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
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int templateID)
        {
            List<Post> relatedPosts = await _context.Posts
                .Include(u => u.User)
                .Where(p => p.TemplateID == templateID).Take(10).ToListAsync();

            // If the Template does not relate we return random Posts
            if (relatedPosts.Count < 10)
            {
                relatedPosts = await _context.Posts
                    .Include(u => u.User)
                    .OrderBy(r => Guid.NewGuid()).Skip(3).Take(10).ToListAsync();
            }

            // Giving the View the related Posts
            return View(relatedPosts);
        }
    }
}