using MemeCo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;using System.Linq;
using System.Threading.Tasks;

namespace MemeCo.Models
{
    public class DbInitializer
    {
        private UserManager<MemeCoUser> _userManager;
        private MemeCoContext _memeCoContext;

        public DbInitializer(UserManager<MemeCoUser> usermanager, MemeCoContext memeCoContext)
        {
            _userManager = usermanager;
            _memeCoContext = memeCoContext;
        }

        /// <summary>
        /// Seeds the memeCoContext with users and posts. Returns if the database has already been seeded.
        /// </summary>
        public async void Seed()
        {
            _memeCoContext.Database.Migrate();

            // DB already seeded 
            if (_memeCoContext.Posts.Any())
            {
                return; 
            }

            // Seed Users
            MemeCoUser user = addUser("testuser", "test@meme.co", "test bio", false);

            var users = new MemeCoUser[]
            {
                addUser("test1", "test1@meme.co", "test1 bio", true),
                addUser("test2", "test2@meme.co", "test2 bio", true),
                addUser("test3", "test3@meme.co", "test3 bio", true),
                addUser("test4", "test4@meme.co", "test4 bio", true),
                addUser("test5", "test5@meme.co", "test5 bio", true),
            };
            _memeCoContext.SaveChanges();

            // add test post by current user
            Post post = new Post();
           
            post.Description = "test description";
            post.Meme = File.ReadAllBytes("wwwroot\\meme_templates\\avengers_level_threat.jpg");
            post.MemeCoUserID = user.Id;
            post.User = user;
            _memeCoContext.Posts.Add(post);

            // add second test user
            user = addUser("testuser2", "testy@gmail.com", "i like eggs", false);
            post = new Post();
            post.Description = "ok boomer";
            post.Meme = File.ReadAllBytes("wwwroot\\meme_templates\\jealous_girlfriend.jpg");
            post.MemeCoUserID = user.Id;
            post.User = user;
            _memeCoContext.Posts.Add(post);


            // add meme templates to DB
            addTemplates();

            _memeCoContext.SaveChanges();
            
        }



        /// <summary>
        /// Adds a user with the given username, email, bio, and darkmode preference to the userManager and returns on success.
        /// All seeded users are assigned the default password "123ABC!@#def"
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="bio"></param>
        /// <param name="darkMode"></param>
        private MemeCoUser addUser(string username, string email, string bio, bool darkMode)
        {
            MemeCoUser user = new MemeCoUser();
            user.UserName = username;
            user.Email = email;
            user.Bio = bio;
            user.DarkMode = darkMode;
            user.EmailConfirmed = true;
            IdentityResult result =  _userManager.CreateAsync(user, "123ABC!@#def").Result;
            return user;
        }


        /// <summary>
        /// Adds a post with the given properties to the database.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="likes"></param>
        /// <param name="meme"></param>
        /// <param name="memeCoUserID"></param>
        /// <param name="comments"></param>
        /// <param name="user"></param>
        private async void addPost(string description, IEnumerable<Like> likes, byte[] meme, string memeCoUserID, IEnumerable<Comment> comments, MemeCoUser user)
        {
            Post post = new Post();
            post.Description = description;
            post.Likes = likes;
            post.Meme = meme;
            post.MemeCoUserID = memeCoUserID;
            post.Comments = comments;
            post.User = user;

            await _memeCoContext.Posts.AddAsync(post);
            _memeCoContext.SaveChanges();
          
        }

        /// <summary>
        /// Adds all templates located in the meme_templates folder to the database. 
        /// </summary>
        private void addTemplates()
        {
            var templates = Directory.GetFiles("wwwroot/meme_templates");
            foreach(var meme in templates)
            {
                Template temp = new Template();
                temp.Content = File.ReadAllBytes(meme);
                temp.name = meme.Split("\\")[1];
                _memeCoContext.Templates.Add(temp);
                
            }

        }
        
    }
}
