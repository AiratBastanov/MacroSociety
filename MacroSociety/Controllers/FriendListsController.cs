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

        [HttpGet("allfriends")]
        public async Task<IEnumerable<FriendList>> GetFriendAll(string myname, int chunkIndex, int chunkSize)
        {
            var MyFriends = await _context.FriendLists
               .Where(friendlist => friendlist.Username == myname)
               .OrderBy(friendlist => friendlist.Id) // Сортировка по возрастанию по полю Id
               .Skip((chunkSize - 1) * chunkIndex)
               .Take(chunkSize)
               .ToListAsync();
            return MyFriends;
        }

       /* [HttpGet("allfriends")]
        public async Task<IEnumerable<FriendList>> GetFriendAll(string myname, int chunkIndex, int chunkSize)
        {
            IEnumerable<FriendList> MyFriends = await _context.FriendLists.Where(friendlist => friendlist.Username == myname).ToListAsync();
            return MyFriends;
        }*/

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

