using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[Action]")]
[ApiController]
public class Controllers:Controller
{
    ILogginProcces lp;
    public Controllers(ILogginProcces _lp)
    {
        lp=_lp;
    }

    [HttpPost]
    public IActionResult Login(IdLessUser user){
        return Ok(lp.Login(user));
    }

    [HttpPost]
    public IActionResult Register(IdLessUser user){
        return Ok(lp.Register(user));
    }
    
    [HttpPut]
    public IActionResult RestorePassword(UpdateUser user){
        return Ok(lp.UpdatePassword(user));
    }

    [HttpGet]
    [Authorize(Roles ="user,admin")]
    public IActionResult SelectFruits(){
        List<string> fruits = new List<string>{"sib","kivi","moz","ANNANNAS!","holo","gilas"};
        return Ok(fruits); 
    }

    [HttpDelete]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteUser(int Id){
        return Ok(lp.DeleteUser(Id));
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public IActionResult AllUser(){
        return Ok(lp.AllUser());
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public IActionResult Promote(int UserID , string Role){
        return Ok(lp.Promote(UserID , Role));
    }
}