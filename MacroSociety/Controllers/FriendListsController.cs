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
        public ActionResult<FriendList> GetFriend(string myname, string friendname)
        {          
            FriendList FriendList = _context.FriendLists.Where(friendlist => friendlist.Username == myname && friendlist.Friendname == friendname).FirstOrDefault();
            if (FriendList == null)
                return NotFound();
            return Ok(FriendList);
        }
        [HttpGet("allfriends")]
        public IEnumerable<FriendList> GetFriendAll(string myname)
        {
            IEnumerable<FriendList> MyFriends;
            MyFriends = _context.FriendLists.Where(friendlist => friendlist.Username == myname);
            return MyFriends;
        }
        [HttpPost]
        public int CreateNewFriend(FriendList friendlist)
        {
            int ResultPost = 0;
            _context.FriendLists.Add(friendlist);
            ResultPost = _context.SaveChanges();
            _context.FriendLists.Add(new FriendList() { Friendname = friendlist.Username, Username = friendlist.Friendname, IdUsername = friendlist.IdFriendname, IdFriendname = friendlist.IdUsername });
            ResultPost += _context.SaveChanges();
            return ResultPost;
        }
    }
}

