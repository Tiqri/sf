using Blissmo.UserService.Interface.Model;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blissmo.UserService
{
    interface IUserRepository : IService
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUser(Guid userId);

        Task AddUser(User user);
    }
}
