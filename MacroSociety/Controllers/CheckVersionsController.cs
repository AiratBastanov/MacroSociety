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
    [Route("api/version")]
    public class CheckVersionsController : Controller
    {
        private readonly MacroSocietyContext _context;

        public CheckVersionsController(MacroSocietyContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<string> GetVersion()
        {
            return (await _context.CheckVersions.FirstOrDefaultAsync())?.LastVersion;
        }
    }
}
