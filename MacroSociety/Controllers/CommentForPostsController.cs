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
        public async Task<IEnumerable<CommentForPost>> GetComment(int userId, int postId, string commentType, int chunkIndex, int chunkSize)//int
        {
            IEnumerable<CommentForPost> comments;
            //int count = 0;

            if (commentType == "all")
            {
                comments = await _context.CommentForPosts
                    .Where(comment => comment.IdFriendPost == postId)
                    .OrderBy(comment => comment.Id)
                    .Skip((chunkSize - 1) * chunkIndex)
                    .Take(chunkSize)
                    .ToListAsync();
            }
            else//commentType == "friends"
            {             
                var friends = await _context.FriendLists
            .Where(f => (f.IdUsername == userId))
            .Select(f => f.IdFriendname == userId ? f.IdUsernameNavigation : f.IdFriendnameNavigation)
            .ToListAsync();
               // count = friends.Count();
                var friendIds = friends.Select(f => f.Name).ToList();
                // Выбираем комментарии только от друзей к данному посту
                comments = await _context.CommentForPosts
            .Where(c => c.IdFriendPost == postId && friendIds.Contains(c.NameUserComment))
            .Skip(chunkIndex * chunkSize)
            .Take(chunkSize)
            .ToListAsync();
            }

                return comments;
                //return count;
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
