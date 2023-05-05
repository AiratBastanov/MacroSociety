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
    [Route("api/request")]
    public class FriendRequestsController : Controller
    {
        private readonly MacroSocietyContext _context;

        public FriendRequestsController(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<FriendRequest>> GetInfoReq(string username)
        {
            FriendRequest FriendRequest = await _context.FriendRequests.FirstOrDefaultAsync(friendRequest => friendRequest.FutureFriend == username);
            if (FriendRequest == null)
            {
                return NotFound();
            }
            return Ok(FriendRequest);
        }

        [HttpGet("futurefriend")]
        public async Task<ActionResult<FriendRequest>> GetFutureFriend(string futurefriend)
        {
            FriendRequest friendRequest = await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.UserName == futurefriend);
            if (friendRequest == null)
            {
                return NotFound();
            }
            return Ok(friendRequest);
        }

        [HttpGet("requestlist")]
        public async Task<IEnumerable<FriendRequest>> GetFriendRequestListAsync(string myname)
        {
            IEnumerable<FriendRequest> friendRequests = await _context.FriendRequests.Where(friendRequest => friendRequest.FutureFriend == myname).ToListAsync();
            return friendRequests;
        }

        [HttpPost]
        public async Task<int> PostNewRequest(FriendRequest friendRequest)
        {
            _context.FriendRequests.Add(friendRequest);
            return await _context.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            int ResultDelete = 0;
            FriendRequest FriendRequest = await _context.FriendRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (FriendRequest != null)
            {
                _context.FriendRequests.Remove(FriendRequest);
                ResultDelete = await _context.SaveChangesAsync();
            }
            return ResultDelete;
        }
    }
}