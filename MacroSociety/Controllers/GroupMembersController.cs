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
    [Route("api/groupMember")]
    [ApiController]
    public class GroupMembersController : ControllerBase
    {
        private readonly MacroSocietyContext _context;

        public GroupMembersController(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupMember>>> GetGroupMembers()
        {
            return await _context.GroupMembers.ToListAsync();
        }

        [HttpGet("{groupId}/{userId}")]
        public async Task<ActionResult<GroupMember>> GetGroupMember(int groupId, int userId)
        {
            var member = await _context.GroupMembers.FindAsync(groupId, userId);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        [HttpPost]
        public async Task<ActionResult<GroupMember>> PostGroupMember(GroupMember member)
        {
            _context.GroupMembers.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupMember", new { groupId = member.GroupId, userId = member.UserId }, member);
        }

        [HttpPut("{groupId}/{userId}")]
        public async Task<IActionResult> PutGroupMember(int groupId, int userId, GroupMember member)
        {
            if (groupId != member.GroupId || userId != member.UserId)
            {
                return BadRequest();
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupMemberExists(groupId, userId))
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

        [HttpDelete("{groupId}/{userId}")]
        public async Task<IActionResult> DeleteGroupMember(int groupId, int userId)
        {
            var member = await _context.GroupMembers.FindAsync(groupId, userId);
            if (member == null)
            {
                return NotFound();
            }

            _context.GroupMembers.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupMemberExists(int groupId, int userId)
        {
            return _context.GroupMembers.Any(e => e.GroupId == groupId && e.UserId == userId);
        }
    }
}