using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IPasswordRepo
    {
        public string ResetPasswordMethod(ResetPassword resetPassword);
        public TokenEmailClass ForgetPassWordMethod(string Email);


    }
}
