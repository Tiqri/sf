using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blissmo.UserService.Interface.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Blissmo.UserService
{
    class ServiceFabricUserRepository : IUserRepository
    {
        private readonly IReliableStateManager _stateManager;

        public ServiceFabricUserRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddUser(User user)
        {
            var users = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("users");

            using (var tx = _stateManager.CreateTransaction())
            {
                await users.AddOrUpdateAsync(tx, user.Id, user, (id, value) => user);
                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("users");
            var result = new List<User>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allProducts = await users.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allProducts.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, User> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }

        public async Task<User> GetUser(Guid userId)
        {
            var users = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("users");

            using (var tx = _stateManager.CreateTransaction())
            {
                ConditionalValue<User> user = await users.TryGetValueAsync(tx, userId);

                return user.HasValue ? user.Value : null;
            }
        }
    }
}
