using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IPasswordBusiness
    {
        public string ResetPasswordMethod(ResetPassword resetPassword);

        public TokenEmailClass FogetPasswordMethod(string Email);

    }
}
