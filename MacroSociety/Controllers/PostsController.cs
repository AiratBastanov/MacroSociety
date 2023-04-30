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
        public IEnumerable<Post> GetPostFriend(string name)
        {
            IEnumerable<Post> myAL;
            myAL = _context.Posts.Where(post => post.NameUser == name);
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
        public int CreateNewUser(Post post)
        {
            int result = 0;
            _context.Posts.Add(post);
            result = _context.SaveChanges();         
            return result;
        }
        [HttpGet("myposts")]
        public IEnumerable<Post> GetMyPost(string name)
        {
            IEnumerable<Post> myAL;
            myAL = _context.Posts.Where(post => post.NameUser == name);
            return myAL;
        }
        [HttpDelete("deletepost")]
        public int Delete(int id)
        {
            int result = 0;
            Post post = _context.Posts.FirstOrDefault(x => x.Id == id);            
            if (post != null)
            {
                _context.Posts.Remove(post);
                result = _context.SaveChanges();
            }
            List<Like> like;
            like = _context.Likes.Where(like=>like.IdFriendPost == id).ToList();
            if (like != null)
            {
                for (int i = 0; i < like.Count(); ++i)
                {
                    _context.Likes.Remove(like.ElementAt(i));
                    result = _context.SaveChanges();
                }
            }            
            List<CommentForPost> comment;
            comment = _context.CommentForPosts.Where(like => like.IdFriendPost == id).ToList();
            if (comment != null)
            {
                for (int i = 0; i < comment.Count(); i++)
                {
                    _context.CommentForPosts.Remove(comment.ElementAt(i));
                    result = _context.SaveChanges();
                    //result = comment.Count();
                }
            }                            
            return result;
        }
    }
}
