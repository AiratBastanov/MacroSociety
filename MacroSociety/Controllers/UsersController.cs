using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MacroSociety.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppMacroSociety.EmailServies;
using WebAppMacroSociety.Randoms;

namespace WebAppMacroSociety.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly MacroSocietyContext _context;
        private CreateVerificationCode createVerificationCode;
        private EmailService emailService;
        private int VerificationCode=0;

        public UsersController(MacroSocietyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUserByLogin(string name)
        {
            User user = await _context.Users.FirstOrDefaultAsync(user => user.Name == name);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("allusers")]
        public async Task<IEnumerable<User>> GetUsers(string myname, int chunkIndex, int chunkSize)
        {
            var myAL = await _context.Users
                .Where(user => user.Name != myname && !_context.FriendLists
                    .Any(friend => (friend.Username == myname && friend.Friendname == user.Name) ||
                                   (friend.Friendname == myname && friend.Username == user.Name)))
                .Skip((chunkSize - 1) * chunkIndex)
                .Take(chunkSize)
                .ToListAsync();
            return myAL;
        }


        [HttpGet("checkemail")]
        public async Task<int> GetEmailandCheck(string email)
        {
            emailService = new EmailService();
            createVerificationCode = new CreateVerificationCode();
            VerificationCode = createVerificationCode.RandomInt(6);
            string bodyMessage = @"Проверочный код: " + VerificationCode.ToString();
            await emailService.SendEmailAsync(email, "Шайтан-машина", bodyMessage);
            return VerificationCode;
        }
        [HttpGet("sendcomment")]
        public async Task<int> GetCommentandSend(string infoUser, string comment)
        {
            emailService = new EmailService();
            createVerificationCode = new CreateVerificationCode();
            string bodyMessage = @"Текст пользователя: " + comment;
            await emailService.SendEmailAsync("ajratbastanov@gmail.com", infoUser, bodyMessage);
            return 1;
        }
        [HttpPost]
        public async Task<int> CreateNewUser(User user)
        {
            Game game = new Game();
            int ResultPost = 0;
            _context.Users.Add(user);
            ResultPost = await _context.SaveChangesAsync();
            User User = await _context.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
            int IdUser = User.Id;
            game.NameUser = user.Name;
            game.IdUser = IdUser;
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return ResultPost;
        }
       
        [HttpPut]
        public async Task<int> Put(User user)
        {
            int ResultPut = 0;
            if (user == null)
            {
                return ResultPut;
            }
            _context.Update(user);
            ResultPut = await _context.SaveChangesAsync();
            return ResultPut;
        }

        [HttpDelete("{id}")]
        public async Task<int> DeleteAsync(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}

