using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blissmo.API.Model
{
    public class ApiConversation
    {
        public Guid Id { get; set; }

        public List<ApiMessage> Messages { get; set; }

        public List<ApiUser> Participents { get; set; }
    }
}
