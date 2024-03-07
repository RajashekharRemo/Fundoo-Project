using Microsoft.EntityFrameworkCore;
using Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.PasswordFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }

        User user = new User();

        public IEnumerable<User> GetAll()
        {
            var result = _context.OnlineUser2.ToList();
            return result;
        }

        public User GetUser(string FirstNAme)
        {
            var result = _context.OnlineUser2.FirstOrDefault(x => x.First_Name == FirstNAme);
            if (result == null) return null;

            else return result;
        }

        public UserUpdateModel GetUserByEmail(string Email)
        {
            var result = _context.OnlineUser2.FirstOrDefault(x => x.Email == Email);
            if (result == null) return null;

            UserUpdateModel userModel = new UserUpdateModel();
            userModel.First_Name = result.First_Name; userModel.Last_Name = result.Last_Name; userModel.Email = result.Email;
            
            return userModel;
        }


        Credentials credentials = new Credentials();
        public bool Insert(UserModel model)
        {

            UserModel userModel = credentials.Validation(model);

            if (userModel == null) return false;

            user.First_Name = userModel.First_Name; user.Last_Name = userModel.Last_Name;
            user.Email = userModel.Email;
            user.Password = PasswordConversion.EncryptPassword(userModel.Password);

            _context.OnlineUser2.Add(user);
            _context.SaveChanges();
            return true;
        }
        public bool Update(string Email, UserUpdateModel model)
        {
            var userFoundObject = _context.OnlineUser2.FirstOrDefault(s => s.Email == Email);
            if (userFoundObject == null) { return false; }

            userFoundObject.First_Name = model.First_Name; userFoundObject.Last_Name = model.Last_Name; userFoundObject.Email = model.Email;


            _context.Entry(userFoundObject).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public bool Delete(string name)
        {
            var firstname = _context.OnlineUser2.FirstOrDefault(s => s.First_Name == name);

            if (firstname == null) { return false; }
            _context.OnlineUser2.Remove(firstname);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateOrCreate(int id, UserModel model)
        {
            user = _context.OnlineUser2.FirstOrDefault(s => s.Id == id);
            user.First_Name = model.First_Name;
            user.Last_Name = model.Last_Name;
            user.Email = model.Email;
            user.Password = PasswordConversion.EncryptPassword(model.Password);

            //_context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();


            return true;
        }

        public string CreateOrUpdate(int Id, UserModel userModel)
        {
            var user = _context.OnlineUser2.FirstOrDefault(s => s.Id == Id);

            if(user !=null) {
                UpdateOrCreate(Id, userModel);
                return "Updated";
            }else
            {
                Insert(userModel);
                return "Created";
            }
                       
        }
    }
}
