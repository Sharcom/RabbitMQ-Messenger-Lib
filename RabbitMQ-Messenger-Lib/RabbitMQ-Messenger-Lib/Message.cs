using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Messenger_Lib.Types
{
    public class Message
    {
        public Message(Dictionary<string, object> payload, MessageType type, string origin) {
            Payload = payload;
            Type = type;
            Origin = origin;
        }

        public Dictionary<string, object> Payload { get; private set; }
        public MessageType Type { get; private set; }
        public string Origin { get; private set; }
    }
}
