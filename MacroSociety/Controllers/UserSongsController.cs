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
    [ApiController]
    [Route("api/music")]
    public class UserSongsController : Controller
    {
        private readonly MacroSocietyContext _context;

        public UserSongsController(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<int> GetInfoListen(int id)
        {
            int count = await _context.UserSongs.Where(user => user.UserId == id).CountAsync();
            return count;
        }

        [HttpGet("ids")]
        public async Task<int> GetInfoAboutMusic(int iduser, int idsong)
        {
            int count = 0;
            count = await _context.UserSongs.CountAsync(us => us.UserId == iduser && us.SongId == idsong);
            if (count == 0)
                return count;
            return count;
        }

        [HttpGet("list")]
        public async Task<IEnumerable<UserSong>> GetListListen(int id)
        {
            IEnumerable<UserSong> SongList;
            SongList = await _context.UserSongs.Where(user => user.UserId == id).ToListAsync();
            return SongList;
        }

        [HttpPost]
        public async Task<int> CreateNewListen(UserSong usersong)
        {
            int ResultPost = 0;
            Song songcheck = await _context.Songs.FirstOrDefaultAsync(song => song.Id == usersong.SongId);
            if (songcheck == null)
                return ResultPost;
            _context.UserSongs.Add(usersong);
            ResultPost = await _context.SaveChangesAsync();
            return ResultPost;
        }
    }
}
