using Microsoft.EntityFrameworkCore;
using MyUsers.Models;

namespace MyUsers.Infrastructure;
public class ApplicationDbContext : DbContext
{
	public DbSet<User> Users { get; set; }
	
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql("Host=db;Port=5433;Username=user;Password=password;Database=MyUsersDb");
	}
}