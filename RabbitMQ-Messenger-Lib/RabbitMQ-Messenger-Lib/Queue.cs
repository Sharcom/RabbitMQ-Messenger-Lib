using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Messenger_Lib.Types
{
    public struct Queue
    {
        public Queue(string name, EventHandler<Message>? callbackMethod = null, IDictionary<string, object>? arguments = null, bool durable = true, bool exclusive = false, bool autoDelete = false, bool autoAcknowledge = true)
        {
            Name = name;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            CallbackMethod = callbackMethod;
            Arguments = arguments;
            AutoAcknowledge = autoAcknowledge;
        }
        public string Name { get; private set; }
        public bool Durable { get; private set; } = true;
        public bool Exclusive { get; private set; } = false;
        public bool AutoDelete { get; private set; } = false;
        public bool AutoAcknowledge { get; private set; } = true;
        public IDictionary<string, object>? Arguments { get; private set; }
        public EventHandler<Message>? CallbackMethod { get; private set; }
    }
}
