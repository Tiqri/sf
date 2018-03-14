using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blissmo.Helpers.KeyVault;
using Blissmo.SearchService.Interfaces;
using Blissmo.SearchService.Interfaces.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Blissmo.SearchService.SearchProvider;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Data;
using Blissmo.Helpers.QueryHelper;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Blissmo.Helpers.RemotingSerializationProvider;

namespace Blissmo.SearchService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class SearchService : StatefulService, ISearchService
    {
        private ISearchProvider _searchProvider;
        private string _searchServiceName = KeyVault.GetValue("SEARCH_SERVICE_NAME");
        private string _adminApiKey = KeyVault.GetValue("SEARCH_SERVICE_KEY");
        private string _indexName = "movie-index";

        public SearchService(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            /*  Remoting V2 does not support for complex type defaultly
                Here overwriten serialize provider as json serialization to serialize the object */
            return new[]
            {
                new ServiceReplicaListener((context) =>
                {
                    return new FabricTransportServiceRemotingListener(context, this, null,
                        new ServiceRemotingJsonSerializationProvider());
                })
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _searchProvider = new SearchProvider.SearchProvider();

            var searchClient = await _searchProvider.CreateSearchServiceAsync(_searchServiceName, _adminApiKey);
            if (!await _searchProvider.IsAnyIndexExists(searchClient, _indexName))
            {
                await _searchProvider.DeleteIndexIfExistsAsync(searchClient, _indexName);
                await _searchProvider.CreateIndexAsync<Movie>(searchClient, _indexName);

                ISearchIndexClient indexClient = searchClient.Indexes.GetClient(_indexName);
                await _searchProvider.SetDataSourceAsync(indexClient);
            }
        }

        public async Task<IList<Movie>> SearchMovies(Interfaces.Model.SearchParameters searchParameters)
        {
            Microsoft.Azure.Search.Models.SearchParameters parameters;
            DocumentSearchResult<Movie> results;
            IList<Movie> resultSet = new List<Movie>();

            resultSet = await GetMovieFromCache(searchParameters.SearchTerm);
            if (resultSet == null || !resultSet.Any())
            {
                resultSet = new List<Movie>();

                ISearchIndexClient indexClientForQueries =
                  await _searchProvider.CreateSearchIndexAsync(_searchServiceName, _adminApiKey, _indexName);

                parameters =
                    new Microsoft.Azure.Search.Models.SearchParameters()
                    {
                        Select = searchParameters.Select,
                        Filter = searchParameters.Filter,
                        OrderBy = searchParameters.OrderBy,
                        Top = searchParameters.Top,
                    };

                results = await indexClientForQueries.Documents.SearchAsync<Movie>(searchParameters.SearchTerm, parameters);
                results.Results.ToList().ForEach(i =>
                {
                    resultSet.Add(i.Document);
                });

                StoreSeachCache(searchParameters.SearchTerm, resultSet);
            }

            return resultSet;
        }

        private async Task<IList<Movie>> GetMovieFromCache(string searchTerm)
        {
            var movieReliableDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, IList<Movie>>>("SearchCache");

            using (var tx = this.StateManager.CreateTransaction())
            {
                ConditionalValue<IList<Movie>> movieList = await movieReliableDictionary.TryGetValueAsync(tx, searchTerm);

                return movieList.HasValue ? movieList.Value : null;
            }
        }

        private async void StoreSeachCache(string searchTerm, IList<Movie> searchResult)
        {
            var searchState = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, IList<Movie>>>("SearchCache");
            using (ITransaction tx = this.StateManager.CreateTransaction())
            {
                bool addResult = await searchState.TryAddAsync(tx, searchTerm, searchResult);
                await tx.CommitAsync();
            }
        }
    }
}
