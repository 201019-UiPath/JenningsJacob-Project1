using System.Collections.Generic;
using System.Threading.Tasks;
using GGsDB.Models;

namespace GGsDB.Repos
{
    public interface IUserRepo
    {
        void AddUser(User user);
        void UpdateUser(User user);
        List<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByEmail(string email);
        void DeleteUser(int id);
    }
}