using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services.PasswordFolder
{
    public class Credentials
    {
        public UserModel Validation(UserModel userModel)
        {
            var Validate = new ValidationContext(userModel, null, null);
            var result = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(userModel, Validate, result, true);
            if (!isValid)
            {
                return null;
            }
            return userModel;
        }

        /*public UserLoginModel UserLoginValidation(UserLoginModel userModel)
        {
            var Validate = new ValidationContext(userModel, null, null);
            var result = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(userModel, Validate, result, true);
            if (!isValid)
            {
                return null;
            }
            return userModel;
        }*/
    }
}
