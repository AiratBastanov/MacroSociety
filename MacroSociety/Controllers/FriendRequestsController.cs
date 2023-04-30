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
        public ActionResult<FriendRequest> GetInfoReq(string username)
        {
            FriendRequest FriendRequest = _context.FriendRequests.Where(friendRequest => friendRequest.FutureFriend == username).FirstOrDefault();
            if (FriendRequest == null)
                return NotFound();
            return Ok(FriendRequest);
        }
        [HttpGet("futurefriend")]
        public ActionResult<FriendRequest> GetFutureFriend(string futurefriend)
        {
            FriendRequest FriendRequest = _context.FriendRequests.Where(friendRequest => friendRequest.UserName == futurefriend).FirstOrDefault();
            if (FriendRequest == null)
                return NotFound();
            return Ok(FriendRequest);
        }
        [HttpGet("requestlist")]
        public IEnumerable<FriendRequest> GetFriendReauestList(string myname)
        {
            IEnumerable<FriendRequest> FriendRequest = _context.FriendRequests.Where(friendRequest => friendRequest.FutureFriend == myname);
            return FriendRequest;
        }
        [HttpPost]
        public int PostNewRequest(FriendRequest friendRequest)
        {
            int ResultPost = 0;
            _context.FriendRequests.Add(friendRequest);
            ResultPost = _context.SaveChanges();
            return ResultPost;
        }
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
    }
}