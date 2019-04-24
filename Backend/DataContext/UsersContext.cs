using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
    }
}