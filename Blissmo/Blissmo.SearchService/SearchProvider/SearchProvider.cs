using Blissmo.Helper.KeyVault;
using Blissmo.SearchService.Interface.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.SearchService.SearchProvider
{
    public class SearchProvider : ISearchProvider
    {
        public async Task<SearchServiceClient> CreateSearchServiceAsync(string searchServiceName, string searchServiceKey)
        {
            return await Task.FromResult(
                new SearchServiceClient(searchServiceName, new SearchCredentials(searchServiceKey))
            );
        }

        public async Task<SearchIndexClient> CreateSearchIndexAsync(string searchServiceName, string searchServiceKey, string indexName)
        {
            return await Task.FromResult(
                new SearchIndexClient(searchServiceName, indexName, new SearchCredentials(searchServiceKey))
            );
        }
        
        public async Task<bool> IsAnyIndexExists(SearchServiceClient searchClient, string indexName)
        {
            return await searchClient.Indexes.ExistsAsync(indexName);
        }

        public async Task DeleteIndexIfExistsAsync(SearchServiceClient searchClient, string indexName)
        {
            if (await searchClient.Indexes.ExistsAsync(indexName))
                await searchClient.Indexes.DeleteAsync(indexName);
        }

        public async Task CreateIndexAsync<T>(SearchServiceClient searchClient, string indexName) where T : class
        {
            var indexDefinition = new Index()
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<T>()
            };

            await searchClient.Indexes.CreateAsync(indexDefinition);
        }

        public async Task SetDataSourceAsync(ISearchIndexClient searchIndexClient)
        {
            var movies = new Movie[]
            {
                new Movie()
                {
                    tmsId = "1",
                    title = "Project Almanac",
                    longDescription = "David Raskin (Jonny Weston) is a high-school science nerd who dreams of going to MIT. When he and his friends (Sam Lerner, Allen Evangelista) find his late father's plans for a \"temporal displacement device,\" David can't wait to start tinkering. When they finally get the device to work, the teenagers jump at the opportunity to manipulate time in their favor -- but their joy is short-lived when they begin to discover the consequences of their actions.",
                    shortDescription = "Found footage reveals the fate of several teenagers who build a time machine."
                },
                new Movie()
                {
                    tmsId = "2",
                    title = "The SpongeBob Movie: Sponge Out of Water",
                    longDescription = "Life is dandy in Bikini Bottom for SpongeBob Squarepants (Tom Kenny) and his friends Patrick (Bill Fagerbakke), Squidward (Rodger Bumpass), Mr. Krabs (Clancy Brown) and Sandy (Carolyn Lawrence). However, when the top-secret recipe for Krabby Patties is stolen, SpongeBob finds that he must join forces with perpetual adversary Plankton (Mr. Lawrence) and come ashore to battle a fiendish pirate named Burger Beard (Antonio Banderas), who has his own plans for the delicious delicacies.",
                    shortDescription = "SpongeBob and Plankton join forces against a pirate after the recipe for Krabby Patties is stolen."
                },
                new Movie()
                {
                    tmsId = "3",
                    title = "The SpongeBob Movie: Sponge Out of Water 3D",
                    longDescription = "Life is dandy in Bikini Bottom for SpongeBob Squarepants (Tom Kenny) and his friends Patrick (Bill Fagerbakke), Squidward (Rodger Bumpass), Mr. Krabs (Clancy Brown) and Sandy (Carolyn Lawrence). However, when the top-secret recipe for Krabby Patties is stolen, SpongeBob finds that he must join forces with perpetual adversary Plankton (Mr. Lawrence) and come ashore to battle a fiendish pirate named Burger Beard (Antonio Banderas), who has his own plans for the delicious delicacies.",
                    shortDescription = "SpongeBob and Plankton join forces against a pirate after the recipe for Krabby Patties is stolen."
                }
            };

            var batch = IndexBatch.Upload(movies);

            try
            {
                await searchIndexClient.Documents.IndexAsync(batch);
            }
            catch (IndexBatchException e)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and retrying.
                Console.WriteLine(
                    "Failed to index some of the documents: {0}",
                    String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
            }
        }
    }
}
