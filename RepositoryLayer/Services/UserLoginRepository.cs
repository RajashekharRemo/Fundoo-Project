using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private UserContext _context;
        private readonly IConfiguration _config;
        public UserLoginRepository(UserContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        public TokenEmailClass LoginMethod(UserLoginModel userModel)
        {

            //Credentials credentials = new Credentials();

            //UserLoginModel userModel = credentials.UserLoginValidation(model);
            //if (userModel == null) { return false; }

            var user = _context.OnlineUser2.FirstOrDefault(s => s.Email == userModel.Email);
            if (user == null) { return null; }

            userModel.Password = PasswordConversion.EncryptPassword(userModel.Password);
            TokenEmailClass tokenEmailClass = new TokenEmailClass();
            if (userModel.Email.Equals(user.Email) && userModel.Password.Equals(user.Password)) { 
                tokenEmailClass.Id=user.Id;
                tokenEmailClass.Email = user.Email;
                tokenEmailClass.Token= GenerateToken(user.Id, user);
                tokenEmailClass.First_name = user.First_Name;
                tokenEmailClass.Last_Name = user.Last_Name;
                return tokenEmailClass;
            }
            else return null;

        }

        public string GenerateToken(int id, User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim("Email", user.Email),
            new Claim("UserId", id.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /*public string GenerateToken( UserLoginModel userLoginModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"],
                null, expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/


    }
}
