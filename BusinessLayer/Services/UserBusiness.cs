using BusinessLayer.Interfaces;
using Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository repository;
        public UserBusiness(IUserRepository repository)
        {
            this.repository = repository;
        }


        public IEnumerable<User> GetAll()
        {
            return repository.GetAll();
        }

        public User GetUser(string FirstNAme)
        {
            return repository.GetUser(FirstNAme);
        }
        public UserUpdateModel GetUserByEmail(string Email)
        {
            return repository.GetUserByEmail(Email);
        }


        public bool Insert(UserModel model)
        {
            return this.repository.Insert(model);
        }

        public bool Update(string FirstName, UserUpdateModel model)
        {
            return this.repository.Update(FirstName, model);
        }

        public bool DeleteUser(string firstname)
        {
            return this.repository.Delete(firstname);
        }

        
        public string CreateOrUpdate(int Id,UserModel model)
        {
            return this.repository.CreateOrUpdate(Id, model);
        }

    }
}
