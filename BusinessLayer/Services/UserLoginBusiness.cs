using BusinessLayer.Interfaces;
using RepositoryLayer.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserLoginBusiness : IUserLoginBusiness
    {
        private IUserLoginRepository repository;

        public UserLoginBusiness(IUserLoginRepository repository)
        {
            this.repository = repository;
        }

        public TokenEmailClass LoginMethod(UserLoginModel userModel)
        {
            return repository.LoginMethod(userModel);
        }





    }
}
