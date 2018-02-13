using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blissmo.Helper.QueryHelper
{
    public class QueryReliableDictionaryHelper
    {
        public static async Task<IList<KeyValuePair<Guid, T>>> QueryReliableDictionary<T>
            (IReliableStateManager stateManager, string collectionName, Func<T, bool> filter)
        {
            var result = new List<KeyValuePair<Guid, T>>();

            IReliableDictionary<Guid, T> reliableDictionary =
                await stateManager.GetOrAddAsync<IReliableDictionary<Guid, T>>(collectionName);

            using (ITransaction tx = stateManager.CreateTransaction())
            {
                IAsyncEnumerable<KeyValuePair<Guid, T>> asyncEnumerable = await reliableDictionary.CreateEnumerableAsync(tx);
                using (IAsyncEnumerator<KeyValuePair<Guid, T>> asyncEnumerator = asyncEnumerable.GetAsyncEnumerator())
                {
                    while (await asyncEnumerator.MoveNextAsync(CancellationToken.None))
                    {
                        if (filter(asyncEnumerator.Current.Value))
                            result.Add(asyncEnumerator.Current);
                    }
                }
            }
            return result;
        }
    }
}
