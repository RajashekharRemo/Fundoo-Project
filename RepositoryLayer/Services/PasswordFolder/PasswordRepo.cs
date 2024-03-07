using Microsoft.EntityFrameworkCore;
using Model;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services.PasswordFolder
{
    public class PasswordRepo : IPasswordRepo
    {
        private readonly UserContext _context;
        private readonly IUserLoginRepository _loginRepository;
        public PasswordRepo(UserContext context, IUserLoginRepository loginRepository)
        {
            _context = context;
            _loginRepository = loginRepository;
        }
        public string ResetPasswordMethod(ResetPassword resetPassword)
        {
            var UserResult = _context.OnlineUser2.FirstOrDefault(s => s.Email == resetPassword.Email);
            if (UserResult == null) { return "NotFound"; }

            if (resetPassword.NewPassword.Equals(resetPassword.ConfirmPassword))
            {
                UserResult.Password = PasswordConversion.EncryptPassword(resetPassword.ConfirmPassword);

                _context.Entry(UserResult).State = EntityState.Modified;
                _context.SaveChanges();
                return "Saved";
            }
            else return "PasswordNotEqual";
        }

        public TokenEmailClass ForgetPassWordMethod(string Email)
        {
            var user = _context.OnlineUser2.FirstOrDefault(x => x.Email == Email);
            if (user != null)
            {
                TokenEmailClass model = new TokenEmailClass();
                model.Email = user.Email;
                model.Id = user.Id;
                model.First_name = user.First_Name;
                model.Last_Name = user.Last_Name;
                model.Token = _loginRepository.GenerateToken(user.Id, user);

                return model;
            }
            else
            {
                return null;
            }
        }

    }
}
