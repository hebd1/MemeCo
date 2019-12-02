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

            // seed posts
            postDankMemes();

            MemeCoUser user1 =  addUser("testuser1", "fake@meme.co", "i'm the cooliest", true);
            _memeCoContext.SaveChanges();


            // add test post by current user
            MemeCoUser user2 = addUser("testuser2", "test@meme.co", "test bio", false);
            MemeCoUser user3 = addUser("testuser3", "testuser3@meme.co", "bio", true);

            // add user1 as a follower of user2
            IEnumerable<Follow> follows = new List<Follow>();
            addFollower(follows, user2.Id, user2, user1.Id, user1);
            IEnumerable<Like> likes = new List<Like>();
            Post post1 = addPost("test description", likes, File.ReadAllBytes("wwwroot\\meme_templates\\spongebob_burned_note.png"), user2.Id, user2, 5);
            Post post3 = addPost("test", likes, File.ReadAllBytes("wwwroot\\meme_templates\\spongebob_burned_note.png"), user1.Id, user1, 5);
            Post post4 = addPost("test", likes, File.ReadAllBytes("wwwroot\\meme_templates\\spongebob_burned_note.png"), user3.Id, user3, 5);


            // add third test user
            user2 = addUser("testuser3", "testy@gmail.com", "i like eggs", false);
            likes = new List<Like>();
            foreach (MemeCoUser usr in likeUsers)
            {
                addLike(likes, true, usr.Id, post1);
            }
            foreach (MemeCoUser usr in dislikeUsers)
            {
                addLike(likes, false, usr.Id, post1);
            }

            // add second test post
            Post post2 = addPost("ok boomer test description", likes, File.ReadAllBytes("wwwroot\\meme_templates\\jealous_girlfriend.jpg"), user2.Id, user2, 2);
            foreach (MemeCoUser usr in dislikeUsers)
            {
                addLike(likes, false, usr.Id, post2);
            }
            addLike(likes, true, likeUsers[1].Id, post2);

            
            // add meme templates to DB
            addTemplates();

            // Seed comments
            addComment("testuser1 comment number 1", user1, post1);
            addComment("testuser1 comment number 2. This one is going to be super long to test multiple lines showing up. Wow!", user1, post2);
            addComment("testuser2 comment number 1", user2, post1);
            addComment("testuser2 comment number 2", user2, post2);
            addComment("testuser2 comment number 3. Okay now he's angry about this spicy meme. Watch out test1, test2 is coming for you!", user2, post2);
            
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
        private Post addPost(string description, IEnumerable<Like> likes, byte[] meme, string memeCoUserID, MemeCoUser user, int? templateID)
        {
            Post post = new Post();
            post.Description = description;
            post.Likes = likes;
            post.Meme = meme;
            post.MemeCoUserID = memeCoUserID;
            post.User = user;
            post.TemplateID = templateID;

            _memeCoContext.Posts.Add(post);
            _memeCoContext.SaveChanges();
            return post;
          
        }

        /// <summary>
        /// Adds a like associated with the given post and userID
        /// </summary>
        /// <param name="likes"></param>
        /// <param name="liked"></param>
        /// <param name="userID"></param>
        /// <param name="post"></param>
        /// <returns></returns>
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
        /// Adds the given follower user to the given user's list of followers
        /// </summary>
        /// <param name="follows"></param>
        /// <param name="userID"></param>
        /// <param name="user"></param>
        /// <param name="followerId"></param>
        /// <param name="follower"></param>
        /// <returns></returns>
        private Follow addFollower(IEnumerable<Follow> follows, string userID, MemeCoUser user, string followerId, MemeCoUser follower)
        {
            Follow follow = new Follow();
            follow.Follower = follower;
            follow.User = user;
            follow.UserID = userID;
            follow.FollowerID = followerId;
            follows.Append(follow);
            user.Followers = follows;
            _memeCoContext.Follows.Add(follow);
            return follow;
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
                _memeCoContext.Templates.Add(temp);
                
            }

        }

        /// <summary>
        /// Adds all templates located in the meme_templates folder to the database. 
        /// </summary>
        private void postDankMemes()
        {
            var memes = Directory.GetFiles("wwwroot/dank_memes");
            MemeCoUser poster;
            int count = 0;
            Post post;
            IEnumerable<Like> likes;
            IEnumerable<Comment> comments;
            MemeCoUser userOne = _userManager.FindByNameAsync("test1").Result;
            MemeCoUser userTwo = _userManager.FindByNameAsync("test2").Result;

            foreach (var meme in memes)
            {
                poster = addUser("memePoster" + count, "memePoster" + count + "@gmail.com", "poster" + count + " bio", false);
                likes = new List<Like>();
                comments = new List<Comment>();
                post = addPost("Post " + count  + " description", likes, File.ReadAllBytes(meme), poster.Id, poster, null);
                count++;

                // for large profile testing
                post = addPost("Post " + count + " description", likes, File.ReadAllBytes(meme), poster.Id, userOne, null);
                post = addPost("Post " + count + " description", likes, File.ReadAllBytes(meme), poster.Id, userTwo, null);
            }

        }
        /// <summary>
        /// Adds a comment on the post by the user
        /// </summary>
        private Comment addComment(string content, MemeCoUser user, Post post)
        {
            Comment c = new Comment();
            c.Content = content;
            c.MemeCoUserID = user.Id;
            c.PostID = post.ID;
            c.TimeCommented = DateTime.UtcNow;

            _memeCoContext.Comments.Add(c);
            _memeCoContext.SaveChanges();

            return c;
        }
    }
}