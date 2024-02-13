using Model;
using RepositoryLayer.Interfaces;
using BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class PasswordBusiness : IPasswordBusiness
    {
        private readonly IPasswordRepo _passwordRepo;
        public PasswordBusiness(IPasswordRepo passwordRepo)
        {
            _passwordRepo = passwordRepo;
        }
        public string ResetPasswordMethod(ResetPassword resetPassword)
        {
            return _passwordRepo.ResetPasswordMethod(resetPassword);
        }

        public TokenEmailClass FogetPasswordMethod(String email)
        {
            return _passwordRepo.ForgetPassWordMethod(email);
        }

    }
}
