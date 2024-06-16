using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
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

    public string DeleteUser(int userID)
    {
        Users check = db.usersTbl.Find(userID);
        if(check==null){
            return "User Not Found";
        }
        foreach (UserRoles item in db.UserRoleTbl.Where(x=> x.UserID==userID))
        {
            db.UserRoleTbl.Remove(item);
        }
        db.usersTbl.Remove(check);
        db.SaveChanges();
        return "Done";
        
        
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
            return createToken(new User{ID = check.ID , UserName = user.UserName , Password = user.Password});
        }
    }

    public string Promote(int UserID , string Role)
    {
        Users check = db.usersTbl.Find(UserID);
        Roles checkRoll = db.RoleTbl.FirstOrDefault(x=> x.RoleName == Role);
        if(check == null){
            return "User Not Found";
        }
        else if(checkRoll == null){
            return "Invalid Roll";
        }
        // db.UserRoleTbl.Add(new UserRoles{UserID = UserID, UserRollID = checkRoll.ID}); 
        // برای چند رول ، من بلد نیستم چطور یه کاربر چندتا رول داشته باشه
        
        UserRoles result = db.UserRoleTbl.FirstOrDefault(x=> x.UserID == UserID);
        result.UserRollID = checkRoll.ID;
        db.UserRoleTbl.Update(result);
        db.SaveChanges();
        return $"{check.UserName} is {checkRoll.RoleName} Now ...";

    }

    public string Register(IdLessUser user)
    {   
        if(db.usersTbl.Any(x=> x.UserName==user.UserName)){
            return "USER IS ALLREADY ADDED";
        }

        Users result = new Users{
            UserName = user.UserName,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password+papar+user.UserName),
            CreateDate = DateTime.Now
        };

        db.usersTbl.Add(result);
        db.SaveChanges(); //check ...

        int userId = db.usersTbl.FirstOrDefault(x=> x.UserName==user.UserName).ID;

        UserRoles resultRole = new UserRoles{UserID = userId , UserRollID = db.RoleTbl.FirstOrDefault( x=> x.RoleName == "user").ID};
        db.UserRoleTbl.Add(resultRole);
        db.SaveChanges();

        return createToken(new User{ID = userId , UserName = user.UserName , Password = user.Password});

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

    private string createToken(User user){
        SymmetricSecurityKey privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sAlaM HalEt CheTory ? KhoBi ? Inja Chikar Mikoni Hacker Kalak ? Midonesti Kalak az har Taraf Kalake ?!?"));
        SigningCredentials credentials = new SigningCredentials(privateKey , SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken token = new JwtSecurityToken(
            issuer:"ASeM",
            audience:"ASeM",
            claims: new Claim[]{new Claim(ClaimTypes.Name , user.UserName),new Claim(ClaimTypes.Role , db.RoleTbl.Find(db.UserRoleTbl.FirstOrDefault(x=>x.UserID == user.ID).UserRollID).RoleName)},
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials : credentials
        );
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}