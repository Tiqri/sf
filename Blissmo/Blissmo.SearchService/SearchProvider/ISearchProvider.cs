using Microsoft.Azure.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.SearchService.SearchProvider
{
    public interface ISearchProvider
    {
        Task<SearchServiceClient> CreateSearchServiceAsync(string searchServiceName, string searchServiceKey);

        Task<SearchIndexClient> CreateSearchIndexAsync(string searchServiceName, string searchServiceKey, string indexName);

        Task<bool> IsAnyIndexExists(SearchServiceClient searchClient, string indexName);

        Task DeleteIndexIfExistsAsync(SearchServiceClient searchClient, string indexName);

        Task CreateIndexAsync<T>(SearchServiceClient searchClient, string indexName) where T : class;

        Task SetDataSourceAsync(ISearchIndexClient searchIndexClient);
    }
}
