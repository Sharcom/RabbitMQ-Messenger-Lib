namespace RabbitMQ_Messenger_Lib
{
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ_Messenger_Lib.Types;
    using Newtonsoft.Json;

    public class Sender
    {
        private readonly IModel channel;
        private readonly MessengerConfig config;

        public Sender(MessengerConfig _config, List<Queue> queues)
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = _config.HostName,  };
            config = _config;

            IConnection connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: config.Exchange,
                type: "direct",
                durable: true);

            foreach (Queue queue in queues)
            {
                channel.QueueDeclare(                    
                    queue: queue.Name,
                    durable: queue.Durable,
                    exclusive: queue.Exclusive,
                    autoDelete: queue.AutoDelete,
                    arguments: queue.Arguments);

                channel.QueueBind(
                    queue: queue.Name,
                    exchange: config.Exchange,
                    routingKey: queue.Name);
            }            
        }

        public void Send(Message message, string queueName) {
            string json = JsonConvert.SerializeObject(message);
            byte[] body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(
                exchange: config.Exchange ?? string.Empty,
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }
    }
}