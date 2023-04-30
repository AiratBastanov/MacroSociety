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
    [Route("api/comment")]
    public class CommentForPostsController : Controller
    {
        private readonly MacroSocietyContext _context;

        public CommentForPostsController(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<CommentForPost> GetComment(int id)
        {
            IEnumerable<CommentForPost> comment;
            comment = _context.CommentForPosts.Where(com => com.IdFriendPost == id);
            return comment;
        }
        [HttpPost]
        public int CreateNewLike(CommentForPost comment)
        {
            int result = 0;
            _context.CommentForPosts.Add(comment);
            result = _context.SaveChanges();
            if (result == 1)
            {
                CommentForPost comment2 = _context.CommentForPosts.Where(com => com.IdFriendPost == comment.IdFriendPost && com.NameUserComment==comment.NameUserComment).FirstOrDefault();
                result = comment2.Id;
                return result;
            }
            return result;
        }
    }
}
