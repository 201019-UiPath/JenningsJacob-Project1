using GGsDB.Models;

namespace GGsLib
{
    public interface IUserService
    {
        void AddUser(User user);
        void DeleteUser(User user);
        User GetUserByEmail(string email);
        User GetUserById(int id);
        User UpdateUser(User user, int id);
    }
}