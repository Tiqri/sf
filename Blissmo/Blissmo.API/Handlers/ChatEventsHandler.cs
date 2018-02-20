using Blissmo.ChatService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blissmo.API.Handlers
{
    public class ChatEventsHandler : IChatEvents
    {
        public void ConversationUpdated(Guid conversationId, int count)
        {
            throw new NotImplementedException();
        }
    }
}
