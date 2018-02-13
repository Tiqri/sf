using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blissmo.Helper.QueryHelper;
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

        public async Task AddUser(Login user)
        {
            var users = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Login>>("users");

            using (var tx = _stateManager.CreateTransaction())
            {
                await users.AddOrUpdateAsync(tx, user.User.Id, user, (id, value) => user);
                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Login>>("users");
            var result = new List<User>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allUsers = await users.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allUsers.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, Login> current = enumerator.Current;
                        result.Add(current.Value.User);
                    }
                }
            }

            return result;
        }

        public async Task<Login> GetLoginUserAsync(Login login)
        {
            var queryResult = await QueryReliableDictionaryHelper
                .QueryReliableDictionary<Login>(_stateManager, "users", loginState =>
                    login.UserName == loginState.UserName);

            return queryResult != null && queryResult.Any() ? queryResult.FirstOrDefault().Value : null;
        }

        public async Task<User> GetUser(Guid userId)
        {
            var users = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Login>>("users");

            using (var tx = _stateManager.CreateTransaction())
            {
                ConditionalValue<Login> user = await users.TryGetValueAsync(tx, userId);

                return user.HasValue ? user.Value.User : null;
            }
        }
    }
}
