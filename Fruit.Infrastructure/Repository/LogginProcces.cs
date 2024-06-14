using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class LogginProcces : ILogginProcces
{    Context db ;
    string papar = "Salam In Papar Man Hast MASALA";
    public LogginProcces(){
        db = new Context();
    }

    public List<SimpleUser> AllUser()
    {
        List<SimpleUser> results = new List<SimpleUser>();
        foreach(Users item in db.usersTbl.ToList()){
            SimpleUser result = new SimpleUser{ID = item.ID , UserName = item.UserName};
            results.Add(result);
        }
        return results;
    }

    public string DeleteUser(User user)
    {
        Users check = db.usersTbl.Find(user.ID);
        if(check == null){
            return "Invalid ID";
        }
        else if (check.UserName != user.UserName){
            return "Invalid Username";
        }
        else if(!BCrypt.Net.BCrypt.Verify(user.Password + papar + user.UserName , check.Password)){
            return "Invalid Password";
        }
        else{
            db.usersTbl.Remove(check);
            db.SaveChanges();
            return "Done !";
        }
    }

    public string Login(IdLessUser user)
    {
        Users check = db.usersTbl.FirstOrDefault(x=> x.UserName==user.UserName);
        if(check == null){
            return "Invalid User";
        }
        else if(!BCrypt.Net.BCrypt.Verify(user.Password+papar+user.UserName , check.Password )){
            return "Invalid Password";
        }
        else{
            return createToken(new IdLessUser{UserName = user.UserName , Password = user.Password});
        }
    }

    public string Register(IdLessUser user)
    {   
        if(db.usersTbl.Any(x=> x.UserName==user.UserName)){
            return "USER IS ALLREADY ADDED";
        }

        Users result = new Users{
            UserName = user.UserName,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password+papar+user.UserName)
        };
        db.usersTbl.Add(result);
        db.SaveChanges();
        return createToken(user);

    }

    public string UpdatePassword(UpdateUser user)
    {
        Users check = db.usersTbl.Find(user.ID);
        if(check == null){
            return "Invalid Id";
        }
        else if (check.UserName != user.UserName){
            return "Invalid UserName";
        }
        else{
            check.Password = BCrypt.Net.BCrypt.HashPassword(user.NewPassword+papar+user.UserName);
            db.usersTbl.Update(check);
            db.SaveChanges();
            return "Done !";
        }
    }

    private string createToken(IdLessUser user){
        SymmetricSecurityKey privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sAlaM HalEt CheTory ? KhoBi ? Inja Chikar Mikoni Hacker Kalak ? Midonesti Kalak az har Taraf Kalake ?!?"));
        SigningCredentials credentials = new SigningCredentials(privateKey , SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken token = new JwtSecurityToken(
            issuer:"ASeM",
            audience:"ASeM",
            claims: new Claim[]{new Claim(ClaimTypes.Name , user.UserName)},
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials : credentials
        );
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}