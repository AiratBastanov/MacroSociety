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
    [Route("api/groupInvitation")]
    [ApiController]
    public class GroupInvitationsController : ControllerBase
    {
        private readonly MacroSocietyContext _context;

        public GroupInvitationsController(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupInvitation>>> GetGroupInvitations()
        {
            return await _context.GroupInvitations.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GroupInvitation>>> GetGroupInvitation(int id)
        {

            var invitation = await _context.GroupInvitations.Where(p => p.InvitedUserId == id).ToListAsync();

            if (invitation == null)
            {
                return NotFound();
            }

            return invitation;
        }

        [HttpPost]
        public async Task<ActionResult<GroupInvitation>> PostGroupInvitation(GroupInvitation invitation)
        {
            // Проверяем, существует ли пользователь, который отправил приглашение
            var invitedByUser = await _context.Users.FindAsync(invitation.InvitedBy);
            if (invitedByUser == null)
            {
                return BadRequest($"User with ID {invitation.InvitedBy} does not exist.");
            }

            // Проверяем, существует ли пользователь, который получил приглашение
            var invitedUser = await _context.Users.FindAsync(invitation.InvitedUserId);
            if (invitedUser == null)
            {
                return BadRequest($"User with ID {invitation.InvitedUserId} does not exist.");
            }

            // Проверяем, существует ли уже отправленная заявка этому пользователю в данной группе
            var existingInvitation = await _context.GroupInvitations
                .FirstOrDefaultAsync(i => i.GroupId == invitation.GroupId
                                       && i.InvitedUserId == invitation.InvitedUserId);
            if (existingInvitation != null)
            {
                return Conflict("An invitation has already been sent to this user for this group.");
            }

            // Устанавливаем текущую дату и время для InvitationDate
            invitation.InvitationDate = DateTime.UtcNow;

            // Устанавливаем значение для IsAccepted в false
            invitation.IsAccepted = false;

            // Добавляем объект приглашения в контекст
            _context.GroupInvitations.Add(invitation);

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();

            // Возвращаем ответ с созданным приглашением
            return CreatedAtAction("GetGroupInvitation", new { id = invitation.Id }, invitation);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupInvitation(int id, GroupInvitation invitation)
        {
            if (id != invitation.Id)
            {
                return BadRequest();
            }

            _context.Entry(invitation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupInvitationExists(id))
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
        public async Task<IActionResult> DeleteGroupInvitation(int id)
        {
            var invitation = await _context.GroupInvitations.FindAsync(id);
            if (invitation == null)
            {
                return NotFound();
            }

            _context.GroupInvitations.Remove(invitation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupInvitationExists(int id)
        {
            return _context.GroupInvitations.Any(e => e.Id == id);
        }
    }
}
