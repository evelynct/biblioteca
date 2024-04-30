	
using Library.Models;
using Microsoft.EntityFrameworkCore;
 
namespace Library.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Book { get; set; }
 
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=library.sqlite;Cache=Shared");
    }
}