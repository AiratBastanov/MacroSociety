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

        /*[HttpGet]
        public async Task<IEnumerable<CommentForPost>> GetComment(int id)
        {
            IEnumerable<CommentForPost> comment = await _context.CommentForPosts.Where(com => com.IdFriendPost == id).ToListAsync();
            return comment;
        }*/
        [HttpGet]
        public async Task<IEnumerable<CommentForPost>> GetComment(int id, int chunkIndex, int chunkSize)
        {          
            var comment = await _context.CommentForPosts
             .Where(com => com.IdFriendPost == id)
             .OrderBy(com => com.Id) // Сортировка по возрастанию по полю Id
             .Skip((chunkSize - 1) * chunkIndex)
             .Take(chunkSize)
             .ToListAsync();
            return comment;
        }

        [HttpPost]
        public async Task<int> CreateNewLike(CommentForPost comment)
        {
            _context.CommentForPosts.Add(comment);
            await _context.SaveChangesAsync();

            var savedComment = await _context.CommentForPosts
                .FirstOrDefaultAsync(com => com.IdFriendPost == comment.IdFriendPost && com.NameUserComment == comment.NameUserComment);

            return savedComment?.Id ?? 0;
        }
    }
}
