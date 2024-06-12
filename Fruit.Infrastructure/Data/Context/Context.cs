using Microsoft.EntityFrameworkCore;

public class Context:DbContext
{
    public DbSet<Users> usersTbl { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("server=.\\SQL2019;database=UserTest;trusted_connection=true;MultipleActiveResultSets=True;TrustServerCertificate=True");
    }
}