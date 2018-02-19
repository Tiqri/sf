using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Blissmo.Helper.Encryption;
using Blissmo.UserService.Interface;
using Blissmo.UserService.Interface.Model;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Blissmo.UserService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class UserService : StatefulService, IUserService
    {
        private IUserRepository _repo;

        public UserService(StatefulServiceContext context)
            : base(context)
        { }

        public async Task AddUser(Login user)
        {
            await _repo.AddUser(user);
        }

        public async Task<User> Authorize(Login login)
        {
            var user = await _repo.GetLoginUserAsync(login);
            PasswordHash hash = new PasswordHash(Convert.FromBase64String(user.Password));
            if (!hash.Verify(login.Password))
                throw new System.UnauthorizedAccessException();
            
            return user.User;            
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _repo.GetAllUsers();
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _repo.GetUser(userId);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repo = new ServiceFabricUserRepository(this.StateManager);

            var user1 = new Login {
                UserName = "mark",
                Password = Login.SetPasswordHash("1234"),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Mark",
                    Address = "ABC"
                }
            };

            var user2 = new Login
            {
                UserName = "jhon",
                Password = Login.SetPasswordHash("1234"),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Jhon",
                    Address = "ABC"
                }
            };

            var user3 = new Login
            {
                UserName = "abc",
                Password = Login.SetPasswordHash("1234"),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Abc",
                    Address = "ABC"
                }
            };

            await _repo.AddUser(user1);
            await _repo.AddUser(user2);
            await _repo.AddUser(user3);

            IEnumerable<User> all = await _repo.GetAllUsers();
        }
    }
}
