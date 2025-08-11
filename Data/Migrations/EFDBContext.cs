using Microsoft.EntityFrameworkCore;


namespace Psychiatrist_Management_System.Data.Migrations
{
    public class EFDBContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public EFDBContext(DbContextOptions<EFDBContext> options)
            : base(options)
        {
        }
        public DbSet<Psychiatrist_Management_System.Models.User> Users { get; set; }
        public DbSet<Psychiatrist_Management_System.Models.UserType> UserTypes { get; set; }
    }
}