using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Messenger_Lib.Types
{
    public struct Queue
    {
        public string Name;
        public bool Durable;
        public bool Exclusive;
        public bool AutoDelete;
        public IDictionary<string, object> Arguments;
        public EventHandler<Message>? callbackMethod;
    }
}
