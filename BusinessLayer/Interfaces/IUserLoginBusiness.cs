using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserLoginBusiness
    {
        public TokenEmailClass LoginMethod(UserLoginModel userModel);
        //public string GenerateToken(int id, UserLoginModel userLoginModel);


    }
}
