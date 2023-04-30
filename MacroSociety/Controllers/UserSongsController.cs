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
        public int GetInfoListen(int id)
        {
            int count = 0;
            count = _context.UserSongs.Where(user => user.UserId == id).Count();
            return count;
        }

        [HttpGet("ids")]
        public int GetInfoAboutMusic(int iduser,int idsong)
        {
            int count = 0;
            count = _context.UserSongs.Where(us => us.UserId == iduser && us.SongId== idsong).Count();
            if (count == 0)
                return count;
            return count;
        }

        [HttpGet("list")]
        public IEnumerable<UserSong> GetListListen(int id)
        {
            IEnumerable<UserSong> SongList;
            SongList = _context.UserSongs.Where(user => user.UserId == id);
            return SongList;
        }

        [HttpPost]
        public int CreateNewListen(UserSong usersong)
        {
            int ResultPost = 0;
            Song songcheck = _context.Songs.FirstOrDefault(song => song.Id == usersong.SongId);
            if (songcheck == null)
                return ResultPost;        
            _context.UserSongs.Add(usersong);
            ResultPost = _context.SaveChanges();                      
            return ResultPost;
        }
    }
}
