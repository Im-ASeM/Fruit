public interface ILogginProcces
{
    public string Register(IdLessUser user);
    public string Login(IdLessUser user);
    public string DeleteUser(int UserID);
    public List<SimpleUser> AllUser();
    public string VerifyPassword(UpdateUser user);
    public string RestorePassword(string UserName);
    public string Promote(int UserID,string Role);
}