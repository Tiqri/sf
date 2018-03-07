using Blissmo.UserService.Interfaces.Model;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blissmo.UserService.Interfaces
{
    public interface IUserService : IService
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUser(Guid userId);

        Task AddUser(Login user);

        Task<User> Authorize(Login login);
    }
}
