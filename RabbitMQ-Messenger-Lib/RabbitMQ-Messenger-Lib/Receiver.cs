namespace RabbitMQ_Messenger_Lib
{
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ_Messenger_Lib.Types;
    using Newtonsoft.Json;

    public class Receiver
    {
        private readonly IModel channel;
        public List<Queue> Queues { get; private set; }

        public Receiver(MessengerConfig config, List<Queue> queues)
        {
            Queues = queues;
            var factory = new ConnectionFactory { HostName = config.HostName };
            IConnection connection = factory.CreateConnection();
            channel = connection.CreateModel();

            foreach (Queue queue in queues)
            {
                if (queue.callbackMethod != null)
                {
                    channel.QueueDeclare(
                        queue: queue.Name,
                        durable: queue.Durable,
                        exclusive: queue.Exclusive,
                        autoDelete: queue.AutoDelete,
                        arguments: queue.Arguments);
                }
            }

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                Message message = JsonConvert.DeserializeObject<Message>(json);

                Queue sourceQueue = Queues.Find(queue => queue.Name == ea.RoutingKey);
                sourceQueue.callbackMethod(this, message);
            };

            foreach(Queue queue in Queues)
            {
                channel.BasicConsume(queue: queue.Name,
                    autoAck: true,
                    consumer: consumer);
            }            
        }
    }
}
