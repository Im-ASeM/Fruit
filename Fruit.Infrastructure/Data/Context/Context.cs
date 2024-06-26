using Microsoft.EntityFrameworkCore;

public class Context:DbContext
{
    public DbSet<Users> usersTbl { get; set; }
    public DbSet<Permissions> PermissionTbl { get; set; }
    public DbSet<UserRoles> UserRoleTbl { get; set; }
    public DbSet<Roles> RoleTbl { get; set; }
    public DbSet<smsToken> TokenSms { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("server=.\\SQL2019;database=UserTest;trusted_connection=true;MultipleActiveResultSets=True;TrustServerCertificate=True");
    }
}