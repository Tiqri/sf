using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.ChatService.Interfaces
{
    public interface IChatEvents : IActorEvents
    {
        void ConversationUpdated(Guid conversationId, int count);
    }
}
