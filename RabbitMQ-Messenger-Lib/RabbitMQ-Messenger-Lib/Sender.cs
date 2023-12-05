namespace RabbitMQ_Messenger_Lib
{
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ_Messenger_Lib.Types;
    using Newtonsoft.Json;

    public class Sender
    {
        private readonly IModel channel;

        public Sender(MessengerConfig config, List<Queue> queues)
        {
            var factory = new ConnectionFactory { HostName = config.HostName,  };           
            IConnection connection = factory.CreateConnection();
            channel = connection.CreateModel();

            foreach(Queue queue in queues)
            {
                channel.QueueDeclare(                    
                    queue: queue.Name,
                    durable: queue.Durable,
                    exclusive: queue.Exclusive,
                    autoDelete: queue.AutoDelete,
                    arguments: queue.Arguments);
            }            
        }

        public void Send(Message message, string queueName) {
            string json = JsonConvert.SerializeObject(message);
            byte[] body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }
    }
}