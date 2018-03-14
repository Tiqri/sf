using Blissmo.SearchService.Interfaces.Model;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace Blissmo.SearchService.Interfaces
{
    public interface ISearchService : IService
    {
        Task<IList<Movie>> SearchMovies(SearchParameters searchParameters);
    }
}
