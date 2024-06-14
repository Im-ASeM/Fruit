public interface ILogginProcces
{
    public string Register(IdLessUser user);
    public string Login(IdLessUser user);
    public string DeleteUser(User user);
    public List<SimpleUser> AllUser();
    public string UpdatePassword(UpdateUser user);
}