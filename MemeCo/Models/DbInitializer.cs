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
            var likeUsers = new MemeCoUser[]
            {
                addUser("test1", "test1@meme.co", "test1 bio", true),
                addUser("test2", "test2@meme.co", "test2 bio", true),
                addUser("test3", "test3@meme.co", "test3 bio", true),
                addUser("test4", "test4@meme.co", "test4 bio", true),
                addUser("test5", "test5@meme.co", "test5 bio", true),
              

        };
            var dislikeUsers = new MemeCoUser[]
           {
                addUser("test6", "test6@meme.co", "test6 bio", true),
                addUser("test7", "test7@meme.co", "test7 bio", true),
                addUser("test8", "test8@meme.co", "test8 bio", true),
                addUser("test9", "test9@meme.co", "test9 bio", true),
           };

            // TODO delete my temp user while sendgrid isn't working for me
            addUser("eli", "fake@meme.co", "i'm the cooliest", true);

            _memeCoContext.SaveChanges();

            // add test post by current user
            MemeCoUser user = addUser("testuser", "test@meme.co", "test bio", false);
            IEnumerable<Like> likes = new List<Like>();
            IEnumerable<Comment> comments = new List<Comment>();
            Post post1 = addPost("test description", likes, File.ReadAllBytes("wwwroot\\meme_templates\\spongebob_burned_note.png"), user.Id, comments, user);
            

            // add second test user
            user = addUser("testuser2", "testy@gmail.com", "i like eggs", false);
            likes = new List<Like>();
            foreach (MemeCoUser usr in likeUsers)
            {
                addLike(likes, true, usr.Id, post1);
            }
            foreach (MemeCoUser usr in dislikeUsers)
            {
                addLike(likes, false, usr.Id, post1);
            }

            // add second post
            Post post2 = addPost("ok boomer test description", likes, File.ReadAllBytes("wwwroot\\meme_templates\\jealous_girlfriend.jpg"), user.Id, comments, user);
            foreach (MemeCoUser usr in dislikeUsers)
            {
                addLike(likes, false, usr.Id, post2);
            }
            addLike(likes, true, likeUsers[1].Id, post2);


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
        private Post addPost(string description, IEnumerable<Like> likes, byte[] meme, string memeCoUserID, IEnumerable<Comment> comments, MemeCoUser user)
        {
            Post post = new Post();
            post.Description = description;
            post.Likes = likes;
            post.Meme = meme;
            post.MemeCoUserID = memeCoUserID;
            post.Comments = comments;
            post.User = user;

            _memeCoContext.Posts.Add(post);
            _memeCoContext.SaveChanges();
            return post;
          
        }

        private Like addLike(IEnumerable<Like> likes, bool liked, string userID, Post post)
        {
            Like like = new Like();
            like.Liked = liked;
            like.MemeCoUserID = userID;
            like.Post = post;
            likes.Append(like);
            _memeCoContext.Likes.Add(like);
            return like;
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
