using Blissmo.SearchService.Interfaces.Model;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blissmo.SearchService.Interfaces
{
    public interface ISearchService : IService
    {
        Task<IList<Movie>> SearchMovies(SearchParameters searchParameters);
    }
}
