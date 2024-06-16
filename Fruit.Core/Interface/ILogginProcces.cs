public interface ILogginProcces
{
    public string Register(IdLessUser user);
    public string Login(IdLessUser user);
    public string DeleteUser(int UserID);
    public List<SimpleUser> AllUser();
    public string UpdatePassword(UpdateUser user);
    public string Promote(int UserID,string Role);
}