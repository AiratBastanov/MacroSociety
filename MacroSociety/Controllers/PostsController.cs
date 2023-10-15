using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MacroSociety.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Newtonsoft.Json;

namespace WebApiSociety.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class PostsController : Controller
    {
        private readonly MacroSocietyContext _context;

        public PostsController(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Post>> GetPostFriend(string name)
        {
            IEnumerable<Post> myAL;
            myAL = await _context.Posts.Where(post => post.NameUser == name).ToListAsync();
            return myAL;
        }

        [HttpGet("myposts")]
        public async Task<IEnumerable<Post>> GetMyPost(string name)
        {
            IEnumerable<Post> myAL;
            myAL = await _context.Posts.Where(post => post.NameUser == name).ToListAsync();
            return myAL;
        }

        [HttpGet("posts-and-likes")]
        public async Task<IEnumerable<PostWithLikes>> GetPostsAndLikesWithPagination(int userId, int? friendId, string name, int page, int pageSize)
        {
            var posts = await _context.Posts.Where(p => friendId.HasValue ? p.IdUser == friendId : p.IdUser == userId)
                .Include(p => p.Likes)
             .OrderByDescending(p => p.Id)
             .Skip((pageSize - 1) * page) // Corrected calculation of Skip
                 .Take(pageSize)
                  .ToListAsync(); // Включаем лайки для постов
            List<PostWithLikes> PostsWithLikes = new List<PostWithLikes>();
            foreach (var post in posts)
            {
                var PostWithLikes = new PostWithLikes
                {
                    Id = post.Id,
                    NamePost = post.NamePost,
                    PhotoUrl = post.PhotoUrl,
                    NameUser = post.NameUser,
                    IdUser=post.IdUser,
                    TotalLikes = post.Likes.Count,
                    Likes = post.Likes.Select(like => like.NameUserLike).ToList()
                };
                PostsWithLikes.Add(PostWithLikes);
            }

            return PostsWithLikes;
            //int totalLikes = likes.Count;
           /* var jsonSerializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = JsonConvert.SerializeObject(posts);
            return Ok(json);*/
        }


        [HttpPost]
        public async Task<int> CreateNewPost(Post post)
        {
            _context.Posts.Add(post);
            int result = await _context.SaveChangesAsync();
            return result;
        }

        [HttpDelete("deletepost")]
        public async Task<bool> Delete(int id)
        {
            Post post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                return false;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
