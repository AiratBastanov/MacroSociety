using System;
using System.Collections;
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
    [Route("api/like")]
    public class LikesController : Controller
    {
        private readonly MacroSocietyContext _context;

        public LikesController(MacroSocietyContext context)
        {
            _context = context;
        }
     /*    [HttpGet]
         public async Task<IEnumerable<Like>> GetLike(string myname)
         {
             IEnumerable<Like> likes;
             likes = await _context.Likes.Where(like => like.NameUserLike == myname && like.WhosePost == myname).ToListAsync();
             return likes;
         }*/

        [HttpGet]
        public async Task<IEnumerable<Like>> GetLike(string myname)
        {
            IEnumerable<Like> likes;
            likes = await _context.Likes
                .Where(like => like.NameUserLike == myname && myname.Contains(like.NameUserLike))
                .ToListAsync();

            return likes;
        }     

        [HttpGet("forfriend")]
        public async Task<IEnumerable<Like>> GetLikeForFrinedPost(string myname, string friendname)
        {
            IEnumerable<Like> likes;
            likes = await _context.Likes.Where(like => like.NameUserLike == myname && like.WhosePost == friendname).ToListAsync();
            return likes;
        }

        [HttpPost]
        public async Task<int> CreateNewLike(Like newLike)
        {
            _context.Likes.Add(newLike);
            await _context.SaveChangesAsync();

            if (newLike.Id > 0)
            {
                return newLike.Id;
            }
            return 0;
        }

        /* [HttpPut]
         public int Put(Like like)
         {
             int result = 0;
             if (like == null)
             {
                 return result;
             }
             _context.Update(like);
             result = _context.SaveChanges();
             return result;
         }*/
        [HttpDelete]
        public async Task<int> DeleteLike(int id_like)
        {
            int result = 0;
            Like like = await _context.Likes.FirstOrDefaultAsync(l => l.Id == id_like);
            if (like == null)
            {
                return result;
            }
            _context.Likes.Remove(like);
            result = await _context.SaveChangesAsync();
            return result;
        }
    }
}
