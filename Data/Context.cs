using factura_api_security_project.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace factura_api_security_project.Data
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }


    }
}
