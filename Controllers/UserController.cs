using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using team_calendar.Models;
using Microsoft.AspNetCore.Hosting;
using team_calendar.Dtos;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : Controller
{
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    private readonly IConfiguration configuration;
    private static readonly HttpClient Client = new HttpClient();

    public UserController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration
        )
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.configuration = configuration;
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        
        if (result.Succeeded)
        {
            var appUser = userManager.Users.SingleOrDefault(r => r.Email == model.Email);
            string token = GenerateJwtToken(model.Email, appUser);
            return Ok(token);
        }
        
        return BadRequest("Username or password is wrong!");
    }
    
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.Email, 
            Email = model.Email,
        };
        
        var dbUser = await userManager.FindByNameAsync(model.Email);

        if (dbUser != null) 
        {
            return Conflict();
        }

        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, false);
            string token = GenerateJwtToken(model.Email, user);
            return Ok(token);
        }
        
        return BadRequest();
    }

    private string GenerateJwtToken(string email, User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("firstName", user.FirstName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(configuration["Jwt:ExpireDays"]));

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Issuer"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    [HttpPost]
    public async Task<object> Facebook([FromBody] FacebookDto model)
    {   
        String fbAppId = configuration["Facebook:AppId"];
        String fbAppSecret = configuration["Facebook:AppSecret"];
        var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={fbAppId}&client_secret={fbAppSecret}&grant_type=client_credentials");
        var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);

        var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
        var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

        if (!userAccessTokenValidation.Data.IsValid)
        {
            return BadRequest("Invalid facebook token.");
        }

        var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v3.1/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
        var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

        var user = await userManager.FindByEmailAsync(userInfo.Email);

        if (user == null)
        {
            var newUser = new User
            {
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                UserName = userInfo.Email, 
                Email = userInfo.Email,
                FacebookId = userInfo.Id
            };

            var result = await userManager.CreateAsync(newUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

            if (!result.Succeeded) return BadRequest("Failed to register new facebook user.");

            await signInManager.SignInAsync(newUser, false);
        }

        var localUser = await userManager.FindByNameAsync(userInfo.Email);

        if (localUser==null)
        {
            return BadRequest("Failed to create local user account.");
        }
        await signInManager.SignInAsync(user, false);
        return GenerateJwtToken(localUser.Email, localUser);
    }
}