using Blissmo.RecommendMoviesActor.Interfaces;
using Blissmo.RecommendMoviesActor.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blissmo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Movies")]
    public class MoviesController : Controller
    {
        private IRecommendMoviesActor _recommendMoviesActor;
        private static Uri _recommandMoviesServiceUri = new Uri("fabric:/Blissmo/RecommendMoviesActorService");

        // POST api/Booking
        public async Task<IEnumerable<Movie>> GetRecommandMovies(Guid userId)
        {
            _recommendMoviesActor =
                    ActorProxy.Create<IRecommendMoviesActor>(new ActorId(userId), _recommandMoviesServiceUri);
            return await _recommendMoviesActor.GetUserMovies(CancellationToken.None);
        }
    }
}