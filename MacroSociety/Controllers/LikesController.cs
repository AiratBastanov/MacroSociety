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
        [HttpGet]
        public IEnumerable<Like> GetLike(string myname)
        {
            IEnumerable<Like> likes;
            likes = _context.Likes.Where(like => like.NameUserLike == myname && like.WhosePost == myname);               
            return likes;
        }
        [HttpGet("forfriend")]
        public IEnumerable<Like> GetLikeForFrinedPost(string myname,string friendname)
        {
            IEnumerable<Like> likes;
            likes = _context.Likes.Where(like => like.NameUserLike == myname && like.WhosePost == friendname);
            return likes;
        }
        [HttpPost]
        public int CreateNewLike(Like like1)
        {
            int result = 0;
            _context.Likes.Add(like1);
            result = _context.SaveChanges();
            if (result == 1)
            {
                Like like2 = _context.Likes.Where(like => like.IdFriendPost == like1.IdFriendPost).FirstOrDefault();
                result = like2.Id;
                return result;
            }
            return result;
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
        public int DeleteLike(int id_like)
        {
            int result = 0;
            Like like = _context.Likes.Where(like=>like.Id==id_like).FirstOrDefault();
            if (like == null)
            {               
                return result;
            }
            _context.Likes.Remove(like);
            result = _context.SaveChanges();
            return result;
        }
    }
}
