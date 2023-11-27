using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Messenger_Lib.Types
{
    public class Message
    {
        public Message(string payload, MessageType type) {
            Payload = payload;
            Type = type;
        }

        public string Payload { get; private set; }
        public MessageType Type { get; private set; }
    }
}
