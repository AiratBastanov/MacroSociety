using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MacroSociety.Models;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Hosting;

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
        public async Task<IActionResult> GetPostsAndLikesWithPagination(int userId, int? friendId, string name, int page, int pageSize)//IEnumerable<PostWithLikes> //string
        {/*
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
                    IdUser = post.IdUser,
                    TotalLikes = post.Likes.Count,
                    Likes = post.Likes.Select(like => like.NameUserLike).ToList()
                };
                PostsWithLikes.Add(PostWithLikes);
            }

            return PostsWithLikes;*/
            //второй варинт
            /*  var posts = await _context.Posts.Where(p => friendId.HasValue ? p.IdUser == friendId : p.IdUser == userId)
                  .Include(p => p.Likes)
                  .OrderByDescending(p => p.Id)
                  .Skip((pageSize - 1) * page) // Corrected calculation of Skip
                   .Take(pageSize).ToListAsync();      
              var result = new PostsAndLikes();
              result.Posts = new List<PostWithLikes>();
              foreach (var post in posts)
              {
                  var likes = await _context.Likes.Where(l => l.IdFriendPost == post.Id).ToListAsync();
                  result.Posts.Add(new PostWithLikes(post, likes.Count, likes.ToList()));
              }

             var options = new JsonSerializerOptions
              {
                  ReferenceHandler = ReferenceHandler.Preserve,
                  WriteIndented = true
              };

              var json = JsonSerializer.Serialize(result, options);

              return json;*/
            //третий варинат
            var posts = await _context.Posts.Where(p => friendId.HasValue ? p.IdUser == friendId : p.IdUser == userId)
        .Include(p => p.Likes)
        .OrderByDescending(p => p.Id)
        .Skip((pageSize - 1) * page)
        .Take(pageSize)
        .ToListAsync();

            var postDTOs = posts.Select(p => new PostWithLikes
            {
                Id = p.Id,
                NamePost = p.NamePost,
                PhotoUrl = p.PhotoUrl,
                NameUser = p.NameUser,
                LikeCount = p.Likes.Count,
                Likes = p.Likes.Select(l => new LikeDTO
                {
                    Id = l.Id,
                    NameUserLike = l.NameUserLike,
                    WhosePost = l.WhosePost,
                    IdFriendPost = l.IdFriendPost,
                }).ToList()
            }).ToList();

            return Ok(postDTOs);
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
