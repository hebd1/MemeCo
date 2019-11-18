using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MemeCo.Models;
using Microsoft.AspNetCore.Identity;

namespace MemeCo.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the MemeCoUser class
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

        public MemeCoUser()
        {
            // default profile picture is memeco logo
            ProfilePicture = File.ReadAllBytes("wwwroot\\files\\MemeCo-profile.PNG");
            Filter = "Popular";
        }
    }
}
