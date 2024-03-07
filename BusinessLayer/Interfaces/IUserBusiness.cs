using Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserBusiness
    {
        public IEnumerable<User> GetAll();
        public User GetUser(string FirstNAme);
        public UserUpdateModel GetUserByEmail(string Email);
        public bool Insert(UserModel model);
        public bool Update(string FirstName, UserUpdateModel model);
        public bool DeleteUser(string firstname);

        public string CreateOrUpdate(int Id, UserModel model);
    }
}
