using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blissmo.API.Handlers;
using Blissmo.API.Model;
using Blissmo.ChatService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;


namespace Blissmo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Chat")]
    public class ChatController : Controller
    {
        private static Uri _serviceUri = new Uri("fabric:/Blissmo/ChatServiceActorService");
        private static ActorId _actorId = ActorId.CreateRandom();
        private static IChatService _chatserviceActor = ActorProxy.Create<IChatService>(_actorId, _serviceUri);

        public ChatController()
        {            
        }

        // POST api/Chat [FromBody] ApiMessage message
        [HttpPost]
        public async Task Post()
        {
            await _chatserviceActor.SetCountAsync(1, CancellationToken.None);
        }

        // GET api/Chat
        [HttpGet]
        public async Task Get()
        {
            await _chatserviceActor.SubscribeAsync<IChatEvents>(new ChatEventsHandler());
        }
    }
}