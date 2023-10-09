using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MacroSociety.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

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
        public async Task<IActionResult> GetPostsAndLikesWithPagination(int userId, int? friendId,string name, int page, int pageSize)
        {
            IQueryable<Post> query;

            if (friendId.HasValue)
            {
                query = _context.Posts.Where(post => post.IdUser == friendId);              
            }
            else
            {
                query = _context.Posts.Where(post => post.IdUser == userId);
            }

            query = query.OrderByDescending(post => post.Id);

            var totalPosts = await query.CountAsync();
            var posts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var postIds = posts.Select(post => post.Id).ToList();

            var likes = await _context.Likes
                .Where(like => postIds.Contains(like.IdFriendPost) && like.NameUserLike == name)
                .ToListAsync();

            var postsWithLikes = new PostWithLikes
            {
                Posts = posts,
                Likes = likes
            };

            //var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = JsonSerializer.Serialize(postsWithLikes, jsonSerializerOptions);

            return Ok(json);
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
