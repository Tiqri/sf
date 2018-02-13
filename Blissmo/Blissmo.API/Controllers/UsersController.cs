using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blissmo.API.Model;
using Blissmo.UserService.Interface;
using Blissmo.UserService.Interface.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Blissmo.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController()
        {
            _userService = ServiceProxy.Create<IUserService>(
                new Uri("fabric:/Blissmo/Blissmo.UserService"),
                new ServicePartitionKey(0));
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<ApiUser>> Get()
        {
            var allUsers = await _userService.GetAllUsers();
            return allUsers.Select(u => new ApiUser
            {
                Id = u.Id,
                Name = u.Name,
                Address = u.Address,
                DOB = u.DOB,
                Phone = u.Phone
            }).ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Route("{id:guid}")]
        public async Task<ApiUser> Get(Guid id)
        {
            var user = await _userService.GetUser(id);
            return new ApiUser
            {
                Id = user.Id,
                Name = user.Name,
                Address = user.Address,
                DOB = user.DOB,
                Phone = user.Phone
            };
        }
        
        // POST api/Post
        [HttpPost]
        public async Task Post([FromBody]ApiUser user)
        {
            var newUser = new Login()
            {
                UserName = user.UserName,
                Password = Login.SetPasswordHash(user.Password),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Name = user.Name,
                    Address = user.Address,
                    DOB = user.DOB,
                    Phone = user.Phone
                }
            };

            await _userService.AddUser(newUser);
        }
    }
}
