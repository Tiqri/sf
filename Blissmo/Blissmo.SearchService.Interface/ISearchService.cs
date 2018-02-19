using Blissmo.SearchService.Interface.Model;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.SearchService.Interface
{
    public interface ISearchService : IService
    {
        Task<IList<Movie>> SearchMovies(SearchParameters searchParameters);
    }
}
