using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Blissmo.API.Model;
using Blissmo.RecommendMoviesActor.Interfaces;
using Blissmo.UserService.Interfaces;
using Blissmo.UserService.Interfaces.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Blissmo.API.Controllers
{
    [Route("api/Authorize")]
    public class AuthorizeController : Controller
    {
        private readonly IUserService _userService;
        private static Uri _recommandMoviesServiceUri = new Uri($"{ FabricRuntime.GetActivationContext().ApplicationName }/RecommendMoviesActorService");

        public AuthorizeController()
        {
            _userService = ServiceProxy.Create<IUserService>(
                new Uri($"{ FabricRuntime.GetActivationContext().ApplicationName }/Blissmo.UserService"),
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

            if (loggedUser != null)
            {
                try
                {
                    //Invoke recommand movie actor for loggedin user
                    var recommandMoviesActor =
                           ActorProxy.Create<IRecommendMoviesActor>(new ActorId(loggedUser.Id), _recommandMoviesServiceUri);
                    await recommandMoviesActor.SetUserMovies(CancellationToken.None);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
             

                return new ApiUser
                {
                    Id = loggedUser.Id,
                    Name = loggedUser.Name,
                    Address = loggedUser.Address,
                    Phone = loggedUser.Phone,
                    DOB = loggedUser.DOB
                };
            }

            return null;
        }
    }
}