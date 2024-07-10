using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MacroSociety.Models;

namespace WebAppMacroSociety.Controllers
{
    [ApiController]
    [Route("api/friendlist")]
    public class FriendlistsController : Controller
    {
        private readonly MacroSocietyContext _context;

        public FriendlistsController(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<FriendList>> GetFriend(string myname, string friendname)
        {
            FriendList FriendList = await _context.FriendLists.Where(friendlist => friendlist.Username == myname && friendlist.Friendname == friendname).FirstOrDefaultAsync();
            if (FriendList == null)
                return NotFound();
            return Ok(FriendList);
        }
        [HttpGet("forinvitegroup")]
        public async Task<ActionResult<IEnumerable<User>>> GetFriendsForInviteGroup(string myname, int groupId, int idUser)
        {
            var friendList = await _context.FriendLists
                .Where(fl => fl.Username == myname)
                .Select(fl => fl.IdFriendname)
                .ToListAsync();

            if (!friendList.Any())
                return NotFound();

            var invitedUsers = await _context.GroupInvitations
                .Where(gi => gi.GroupId == groupId && gi.InvitedBy == idUser)
                .Select(gi => new { gi.InvitedUserId, gi.IsAccepted })
                .ToListAsync();

            var filteredFriends = friendList.Where(friendId =>
                !invitedUsers.Any(invite => invite.InvitedUserId == friendId) ||
                invitedUsers.Any(invite => invite.InvitedUserId == friendId && invite.IsAccepted == false))
                .ToList();

            var usersToInvite = await _context.Users
                .Where(u => filteredFriends.Contains(u.Id))
                .ToListAsync();

            return Ok(usersToInvite);
        }


        [HttpGet("allfriends")]
        public async Task<IEnumerable<User>> GetFriendAll(string myname, string friendname, int chunkIndex, int chunkSize)
        {
            IQueryable<FriendList> friendListQuery = _context.FriendLists
                .Where(friendlist => friendlist.Username == myname)
                .OrderBy(friendlist => friendlist.Id);

            if (!string.IsNullOrEmpty(friendname))
            {
                friendListQuery = friendListQuery.Where(friendlist => friendlist.IdFriendnameNavigation.Name.Contains(friendname));
            }
            var MyFriends = await friendListQuery
                .Skip((chunkSize - 1) * chunkIndex)
                .Take(chunkSize)
                .Select(friendlist => friendlist.IdFriendnameNavigation)
                .ToListAsync();
            return MyFriends;
        }

        [HttpGet("allfriends2")]
        public async Task<IEnumerable<User>> GetAllFriends(string myname)
        {
            var MyFriends = await _context.FriendLists
                .Where(friendlist => friendlist.Username == myname)
                .OrderBy(friendlist => friendlist.Id)
                .Select(friendlist => friendlist.IdFriendnameNavigation)
                .ToListAsync();
            return MyFriends;
        }


        [HttpPost]
        public async Task<int> CreateNewFriend(FriendList friendlist)
        {
            int ResultPost = 0;
            _context.FriendLists.Add(friendlist);
            ResultPost = await _context.SaveChangesAsync();
            _context.FriendLists.Add(new FriendList() { Friendname = friendlist.Username, Username = friendlist.Friendname, IdUsername = friendlist.IdFriendname, IdFriendname = friendlist.IdUsername });
            ResultPost += await _context.SaveChangesAsync();
            return ResultPost;
        }
    }
}

