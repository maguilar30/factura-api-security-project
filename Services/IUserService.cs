using factura_api_security_project.Data.Entities;

namespace factura_api_security_project.Services
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User? GetUserByNationalId(string nationalId);
        User? GetUserById(int id);
        User InsertUser(User user);
        User UpdateUser(User user);
        User DeleteUser(int id);
    }
}
