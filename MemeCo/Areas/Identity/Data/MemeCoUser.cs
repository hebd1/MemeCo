/**
 * Author:    Jose Monterroso
 * Partner:   Jasen Lassig, Eli Hebdon
 * Date:      December 6, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jose, Eli, Jasen - This work may not be copied for use in Academic Coursework.
 *
 * I, Jose, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *   MemeCo User
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using MemeCo.Models;
using Microsoft.AspNetCore.Identity;

namespace MemeCo.Areas.Identity.Data
{
    /// <summary>
    /// Add profile data for application users by adding properties to the MemeCoUser class
    /// </summary>
    public class MemeCoUser : IdentityUser
    {
        public string Bio { get; set; }
        [Required]
        public bool DarkMode { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Filter { get; set; }
        public IEnumerable<Follow> Followers { get; set; }
        public IEnumerable<Follow> Following { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Like> Likes { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        // Default profile picture is memeco logo
        public MemeCoUser()
        {
            ProfilePicture = File.ReadAllBytes("wwwroot\\files\\MemeCo-profile.PNG");
            Filter = "Popular";
        }
    }
}