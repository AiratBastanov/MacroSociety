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

        [HttpPost]
        public async Task<int> CreateNewPost(Post post)
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
            return true;
        }
    }
}
