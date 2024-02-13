using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserLoginModel
    {
        [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters")]
        [RegularExpression(@"[A-Za-z0-9!#$%&*'_\-]+@[A-Za-z0-9_]+\.[a-z]{2,3}$", ErrorMessage = "Invalid Email")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^&*_{\-}+'<>?`])[A-Za-z0-9!@#$%^&*_{\-}+'<>?`]{8,16}$", ErrorMessage = "Invalid Password")]
        public string Password { get; set; }
    }
}
