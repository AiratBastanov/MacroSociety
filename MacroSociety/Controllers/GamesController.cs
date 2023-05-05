using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MacroSociety.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class ListLevelUsersController : Controller
    {
        private readonly MacroSocietyContext _context;

        public ListLevelUsersController(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Game>> GetInfoGame(string name)
        {
            Game ListLevelUser = await _context.Games.FirstOrDefaultAsync(listLevelUser => listLevelUser.NameUser == name);
            if (ListLevelUser == null)
                return NotFound();
            return Ok(ListLevelUser);
        }

        [HttpPut]
        public async Task<int> UpdateGameAsync(Game ListLevelUser)
        {
            if (ListLevelUser == null)
            {
                return 0;
            }

            _context.Update(ListLevelUser);
            int result = await _context.SaveChangesAsync();
            return result;
        }
    }
}
