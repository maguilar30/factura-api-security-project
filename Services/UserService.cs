using factura_api_security_project.Data.Entities;
using factura_api_security_project.Repositories;

namespace factura_api_security_project.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User UpdateUser(User user) =>
            _userRepository.UpdateUser(user);

        User IUserService.DeleteUser(int id) =>
            _userRepository.DeleteUser(id);

        List<User> IUserService.GetAllUsers() =>
            _userRepository.GetAllUsers();

        User? IUserService.GetUserByNationalId(string nationalId) =>
            _userRepository.GetUserByNationalId(nationalId);

        User? IUserService.GetUserById(int id) =>
            _userRepository.GetUserById(id);

        User IUserService.InsertUser(User user) =>
            _userRepository.InsertUser(user);
    }
}
