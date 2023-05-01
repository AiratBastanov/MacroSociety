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
        public ActionResult<User> GetUserByLogin(string name)
        {
            User user = _context.Users.FirstOrDefault(user => user.Name == name);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        [HttpGet("allusers")]
        public IEnumerable<User> GetListUsers(string myname)
        {
            IEnumerable<User> UsersList;
            UsersList = _context.Users.Where(user => user.Name != myname);
            return UsersList;
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
        public int CreateNewUser(User user)
        {
            int ResultPost = 0;
            _context.Users.Add(user);
            ResultPost = _context.SaveChanges();
            User User = _context.Users.FirstOrDefault(u => u.Name == user.Name);
            int IdUser = User.Id;            
            PostListgame(user.Name, IdUser);
            return IdUser;
        }
        
        // method create list level user
        public void PostListgame(string nameuser, int IdUser)
        {
            Game game = new Game();
            game.NameUser = nameuser;
            game.IdUser = IdUser;
            _context.Games.Add(game);
            _context.SaveChanges();            
        }

        // PUT for change password       
        [HttpPut]
        public int Put(User user)
        {
            int ResultPut = 0;
            if (user == null)
            {
                return ResultPut;
            }
            _context.Update(user);
            ResultPut = _context.SaveChanges();
            return ResultPut;
        }
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            User User = _context.Users.FirstOrDefault(x => x.Id == id);
            if (User != null)
            {
                _context.Users.Remove(User);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }
    }
}

