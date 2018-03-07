using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blissmo.API.Model;
using Blissmo.SearchService.Interfaces;
using Blissmo.SearchService.Interfaces.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Blissmo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Search")]
    public class SearchController : Controller
    {
        private readonly ILogger _logger;
        private readonly Uri _searchServiceUri = new Uri("fabric:/Blissmo/Blissmo.SearchService");
        private readonly ServicePartitionResolver _servicePartitionResolver = ServicePartitionResolver.GetDefault();

        public SearchController()
        {
            _logger = new LoggerFactory().CreateLogger<EventSource>();
        }

        // GET api/Search
        [HttpGet]
        public async Task<IEnumerable<ApiMovie>> Get(string text)
        {
            try
            {
                var partitionKey = await GetSearchPartitionKeyAsync(text);
                var _searchService = ServiceProxy.Create<ISearchService>(
                    _searchServiceUri,
                    partitionKey);

                var results = await _searchService.SearchMovies(new SearchParameters
                {
                    Select = new[] { "tmsId", "title", "longDescription" },
                    SearchTerm = text
                });

                return results?.Select(i => new ApiMovie()
                    {
                        Id = i.tmsId,
                        Title = i.title,
                        LongDescription = i.longDescription,
                        ShortDescription = i.shortDescription
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<ServicePartitionKey> GetSearchPartitionKeyAsync(string searchTerm)
        {
            char firstLetterOfSearchTerm = searchTerm.First();
            var cancelToken = new CancellationTokenSource();

            if (Char.IsLetter(firstLetterOfSearchTerm))
            {
                var partitionKey = new ServicePartitionKey(Char.ToUpper(firstLetterOfSearchTerm) - 'A');

                var partition = await this._servicePartitionResolver.ResolveAsync(_searchServiceUri, partitionKey, cancelToken.Token);

                Debug.WriteLine("Partition key: '{0}'" +
                    "generated from the first letter '{1}' of input value '{2}'." +
                    "Processing service partition ID: {3}",
                    partitionKey,
                    firstLetterOfSearchTerm,
                    searchTerm,
                    partition.Info.Id
                );

                cancelToken.Cancel(false);
                return partitionKey;
            }

            throw new Exception("Invalid argument!");
        }
    }
}