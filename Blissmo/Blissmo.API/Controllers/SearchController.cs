using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blissmo.API.Model;
using Blissmo.SearchService.Interface;
using Blissmo.SearchService.Interface.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Blissmo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Search")]
    public class SearchController : Controller
    {
        //private readonly ISearchService _searchService;
        private readonly Uri _searchServiceUri = new Uri("fabric:/Blissmo/Blissmo.SearchService");        
        private readonly ServicePartitionResolver _servicePartitionResolver = ServicePartitionResolver.GetDefault();

        public SearchController()
        {
            //_searchService = ServiceProxy.Create<ISearchService>(
            //  _searchServiceUri,
            //  new ServicePartitionKey(0));
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

                return results != null ?
                    results.Select(i => new ApiMovie()
                    {
                        Id = i.tmsId,
                        Title = i.title,
                        LongDescription = i.longDescription,
                        ShortDescription = i.shortDescription
                    })
                    : null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task<ServicePartitionKey> GetSearchPartitionKeyAsync(string searchTerm)
        {
            char firstLetterOfSearchTerm = searchTerm.First();
            if (Char.IsLetter(firstLetterOfSearchTerm))
            {
                ServicePartitionKey partitionKey = new ServicePartitionKey(Char.ToUpper(firstLetterOfSearchTerm) - 'A');
                return partitionKey;
            }

            throw new Exception("Invalid argument!");
            //var cancelToken = new CancellationTokenSource();
            //ResolvedServicePartition resolvedServicePartition = await this._servicePartitionResolver.ResolveAsync(_searchServiceUri, partitionKey, cancelToken.Token);
            //ResolvedServiceEndpoint resolvedServiceEndpoint = resolvedServicePartition.GetEndpoint();
            //resolvedServicePartition.Info.
            //cancelToken.Cancel(false);
        }
    }
}