using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ISearchService _searchService;

        public SearchController()
        {
            _searchService = ServiceProxy.Create<ISearchService>(
              new Uri("fabric:/Blissmo/Blissmo.SearchService"),
              new ServicePartitionKey(0));
        }

        // GET api/Search
        [HttpGet]
        public async Task<IEnumerable<ApiMovie>> Get(string text)
        {
            try
            {
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
    }
}