using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtToken.Models;
using System.Data;
namespace JwtToken.Services
{
    public interface IUserService
    {
        DataTable Authenticate(string username, string password);
        //IEnumerable<User> GetAll();
        //User GetById(int id);
        //User Create(User user, string password);
        //void Update(User user, string password = null);
        //void Delete(int id);
    }
}
