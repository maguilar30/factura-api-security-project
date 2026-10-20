using factura_api_security_project.Data;
using factura_api_security_project.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace factura_api_security_project.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _dbContext;

        public UserRepository(Context dbContext)
        {
            _dbContext = dbContext;
        }

        User IUserRepository.DeleteUser(int id)
        {
            try
            {
                var x = id.ToString();
            }
            catch 
            {
                return new User();
            }
            var data = _dbContext.Users.FirstOrDefault(i => i.Id == id);
            if (data != null)
            {
                data.IsActive = false;
                _dbContext.Users.Update(data);
                _dbContext.SaveChanges();
                return _dbContext.Users.First(i => i.Id == id);
            }
            return new User();
        }

        List<User> IUserRepository.GetAllUsers() =>
            _dbContext.Users.Where(i => i.IsActive).ToList();


        User? IUserRepository.GetUserByNationalId(string nationalId) =>
            _dbContext.Users.Where(i => i.NationalId == nationalId).FirstOrDefault();

        User? IUserRepository.GetUserById(int id) =>
            _dbContext.Users.Where(i => i.Id == id).FirstOrDefault();

        User IUserRepository.InsertUser(User user)
        {
            if (_dbContext.Users.FirstOrDefault(i => i.NationalId == user.NationalId) == null )
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return _dbContext.Users.First(i => i.NationalId == user.NationalId);
            }

            return new User();
           
        }

        User IUserRepository.UpdateUser(User user)
        {
            var data = _dbContext.Users.FirstOrDefault(i => i.Id == user.Id);
            if (data != null)
            {
                data.Name = user.Name;
                data.LastName = user.LastName;
                data.CountryId = user.CountryId;
                data.Email = user.Email;
                data.PhoneNumber = user.PhoneNumber;
                data.Address = user.Address;
                data.IsActive = user.IsActive;
                _dbContext.Users.Update(data);
                _dbContext.SaveChanges();
                return _dbContext.Users.First(i => i.Id == user.Id);
            }

            return new User();
        }
    }
}
