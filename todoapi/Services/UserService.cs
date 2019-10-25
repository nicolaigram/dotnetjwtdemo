using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using todoapi.Data;
using todoapi.Helpers;
using todoapi.Models;

namespace todoapi.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetUserFromToken(string token);

    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        TodoListContext _context;

        public UserService(IOptions<AppSettings> appSettings, TodoListContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                    new Claim("userid", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public User GetUserFromToken(string token)
        {
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var id = jsonToken.Claims.First(claim => claim.Type == "userid").Value;
            var parsedId = int.Parse(id);
            return _context .Users.First(user => user.Id == parsedId);
        }

        public IEnumerable<User> GetAll()
        {
            // TODO: return users without passwords
            return _context.Users.ToList();
        }
    }
}
