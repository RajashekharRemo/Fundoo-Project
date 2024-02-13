using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        public IEnumerable<UserModel> GetAll();
        public UserModel GetUser(string FirstNAme);
        public UserUpdateModel GetUserByEmail(string Email);
        public bool Insert(UserModel model);
        public bool Update(string FirstName, UserUpdateModel model);
        public bool Delete(string name);
        public string CreateOrUpdate(int Id, UserModel model);

    }
}
