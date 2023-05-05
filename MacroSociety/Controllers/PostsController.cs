using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MacroSociety.Models;

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

        /*  [HttpGet]
          public IEnumerable<Post> GetPostFriend(string name, int skip, int take)
          {
              IEnumerable<Post> myAL;
              myAL = _context.Posts.Where(post => post.NameUser == name).Skip(skip).Take(take);
              return myAL;
          }*/
        [HttpPost]
        public async Task<int> CreateNewUser(Post post)
        {
            _context.Posts.Add(post);
            int result = await _context.SaveChangesAsync();
            return result;
        }

        [HttpGet("myposts")]
        public async Task<IEnumerable<Post>> GetMyPost(string name)
        {
            IEnumerable<Post> myAL;
            myAL = await _context.Posts.Where(post => post.NameUser == name).ToListAsync();
            return myAL;
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

            List<Like> likes = await _context.Likes.Where(like => like.IdFriendPost == id).ToListAsync();
            _context.Likes.RemoveRange(likes);
            await _context.SaveChangesAsync();

            List<CommentForPost> comments = await _context.CommentForPosts.Where(comment => comment.IdFriendPost == id).ToListAsync();
            _context.CommentForPosts.RemoveRange(comments);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
