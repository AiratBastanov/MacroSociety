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
    [Route("api/checkfuturefriend")]
    public class FriendRequests2Controller : Controller
    {
        private readonly MacroSocietyContext _context;

        public FriendRequests2Controller(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<FriendRequest>> GetFutureFriend(string futurefriend)
        {
            FriendRequest FriendRequest = await _context.FriendRequests.FirstOrDefaultAsync(friendRequest => friendRequest.UserName == futurefriend);
            if (FriendRequest == null)
                return NotFound();
            return Ok(FriendRequest);
        }
        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            int ResultDelete = 0;
            FriendRequest FriendRequest = await _context.FriendRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (FriendRequest == null)
            {
                return ResultDelete;
            }
            _context.FriendRequests.Remove(FriendRequest);
            ResultDelete = await _context.SaveChangesAsync();
            return ResultDelete;
        }

        [HttpGet("requestlist")]
        public async Task<IEnumerable<FriendRequest>> GetFriendRequestListAsync(string myname)
        {
            IEnumerable<FriendRequest> friendRequests = await _context.FriendRequests.Where(friendRequest => friendRequest.FutureFriend == myname).ToListAsync();
            return friendRequests;
        }
    }
}
