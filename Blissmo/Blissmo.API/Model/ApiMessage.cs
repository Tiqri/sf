using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blissmo.API.Model
{
    public class ApiMessage
    {
        public Guid ConversationId { get; set; }

        public Guid Id { get; set; }

        public string Message { get; set; }

        public ApiUser SentBy { get; set; }

        public DateTime SentAt { get; set; }
    }
}
