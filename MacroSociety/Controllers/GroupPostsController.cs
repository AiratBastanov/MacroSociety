using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MacroSociety.Models;

namespace MacroSociety.Controllers
{
    [Route("api/groupPost")]
    [ApiController]
    public class GroupPostsController : ControllerBase
    {
        private readonly MacroSocietyContext _context;

        public GroupPostsController(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupPost>>> GetGroupPosts()
        {
            return await _context.GroupPosts.ToListAsync();
        }

        [HttpGet("{groupId}")]
        public async Task<ActionResult<IEnumerable<GroupPost>>> GetGroupPosts(int groupId)
        {
            var posts = await _context.GroupPosts.Where(p => p.GroupId == groupId).ToListAsync();

            if (posts == null || !posts.Any())
            {
                return NotFound();
            }

            return posts;
        }


        [HttpPost]
        public async Task<ActionResult<GroupPost>> PostGroupPost(GroupPost post)
        {
            _context.GroupPosts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupPost", new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupPost(int id, GroupPost post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupPost(int id)
        {
            var post = await _context.GroupPosts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.GroupPosts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupPostExists(int id)
        {
            return _context.GroupPosts.Any(e => e.Id == id);
        }
    }
}