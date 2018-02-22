using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blissmo.API.Handlers
{
    public interface IEventHandler
    {
        void Register();

        void Deregister();
    }
}
