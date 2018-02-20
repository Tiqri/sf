using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.ChatService.Interfaces.Model
{
    public class Message
    {
        public Guid ConversationId { get; set; }

        public Guid Id { get; set; }

        public string MessageContent { get; set; }

        public int UserId { get; set; }

        public DateTime SentAt { get; set; }
    }
}
