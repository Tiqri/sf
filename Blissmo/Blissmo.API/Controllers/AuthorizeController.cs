using System;
using System.Threading.Tasks;
using Blissmo.API.Model;
using Blissmo.UserService.Interface;
using Blissmo.UserService.Interface.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Blissmo.API.Controllers
{
    [Route("api/Authorize")]
    public class AuthorizeController : Controller
    {
        private readonly IUserService _userService;

        public AuthorizeController()
        {
            _userService = ServiceProxy.Create<IUserService>(
                new Uri("fabric:/Blissmo/Blissmo.UserService"),
                new ServicePartitionKey(0));
        }

        // POST api/Authorize
        [HttpPost]
        public async Task<ApiUser> Post([FromBody]ApiUser user)
        {
            var loggedUser = await _userService.Authorize(new Login
            {
                UserName = user.UserName,
                Password = user.Password
            });

            return loggedUser != null ? new ApiUser
            {
                Name = loggedUser.Name,
                Address = loggedUser.Address,
                Phone = loggedUser.Phone,
                DOB = loggedUser.DOB
            } : null;
        }
    }
}