using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using Blissmo.RecommendMoviesActor.Interfaces;
using System.IO;
using Newtonsoft.Json;
using Blissmo.RecommendMoviesActor.Interfaces.Models;

namespace Blissmo.RecommendMoviesActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class RecommendMoviesActor : Actor, IRecommendMoviesActor
    {
        /// <summary>
        /// Initializes a new instance of RecommendMoviesActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public RecommendMoviesActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
            return base.OnActivateAsync();
        }

        private List<Movie> GetMovieList()
        {
            using (FileStream s = File.Open(@"D:\movies.json", FileMode.Open))
            using (StreamReader file = new StreamReader(s))
            {
                return JsonConvert.DeserializeObject<List<Movie>>(file.ReadToEnd());
            }
        }

        private List<Movie> GetRecommandMoviesForUser(List<Movie> movies)
        {
            Random rnd = new Random();
            var resultList = new List<Movie>();
            var movieCount = rnd.Next(1, 6);

            while (resultList.Count < movieCount)
            {
                var movie = movies[rnd.Next(movies.Count)];
                if (!resultList.Contains(movie))
                    resultList.Add(movie);
            }

            return resultList;
        }

        /// <summary>
        /// TODO: Replace with your own actor method.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Movie>> GetUserMovies(CancellationToken cancellationToken)
        {
            return await this.StateManager.GetStateAsync<List<Movie>>("RecommandMovies");
        }

        public async Task SetUserMovies(CancellationToken cancellationToken)
        {
            var movieList = GetMovieList();
            var recommandMovieList = GetRecommandMoviesForUser(movieList);

            await this.StateManager.TryAddStateAsync("RecommandMovies", recommandMovieList);
        }
    }
}
