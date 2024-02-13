using Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IUserLoginRepository
    {
        public TokenEmailClass LoginMethod(UserLoginModel userModel);
        //public string GenerateToken(int id,UserLoginModel userLoginModel);

        public string GenerateToken(int Id, User user);



    }
}
