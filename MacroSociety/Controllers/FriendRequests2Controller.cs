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
        public ActionResult<FriendRequest> GetFutureFriend(string futurefriend)
        {
            FriendRequest FriendRequest = _context.FriendRequests.Where(friendRequest => friendRequest.UserName == futurefriend).FirstOrDefault();
            if (FriendRequest == null)
                return NotFound();
            return Ok(FriendRequest);
        }
        /*[HttpDelete("{id}")]*/
        [HttpDelete]
        public int Delete(int id)
        {
            int ResultDelete = 0;
            FriendRequest FriendRequest = _context.FriendRequests.FirstOrDefault(x => x.Id == id);
            if (FriendRequest == null)
            {
                return ResultDelete;
            }
            _context.FriendRequests.Remove(FriendRequest);
            ResultDelete = _context.SaveChanges();
            return ResultDelete;
        }
        [HttpGet("requestlist")]
        public IEnumerable<FriendRequest> GetFriendReauestList(string myname)
        {
            IEnumerable<FriendRequest> FriendRequest = _context.FriendRequests.Where(friendRequest => friendRequest.FutureFriend == myname);
            return FriendRequest;
        }
    }
}
