using System;
using System.Collections.Generic;
using GGsDB.Repos;
using GGsDB.Models;

namespace GGsLib
{
    public class UserService : IUserService
    {
        private IUserRepo repo;
        public UserService(IUserRepo repo)
        {
            this.repo = repo;
        }
        public void AddUser(User user)
        {
            List<User> allUsers = repo.GetAllUsers();
            foreach (var u in allUsers)
            {
                if (u.email.Equals(user.email))
                    throw new Exception("This email already exists.");
            }
            repo.AddUser(user);
        }
        public void DeleteUser(User user)
        {
            List<User> allUsers = repo.GetAllUsers();
            if (!allUsers.Contains(user))
                throw new Exception("This user does not exist and this cannot be deleted.");
            repo.DeleteUser(user);
        }
        public User GetUserByEmail(string email)
        {
            return repo.GetUserByEmail(email);
        }
        public User GetUserById(int id)
        {
            return repo.GetUserById(id);
        }
        public User UpdateUser(User user, int id)
        {
            return repo.UpdateUserLocationId(user, id);
        }
    }
}
