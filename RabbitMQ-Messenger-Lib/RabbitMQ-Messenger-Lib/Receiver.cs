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
        private readonly MessengerConfig config;

        public List<Queue> Queues { get; private set; }

        public Receiver(MessengerConfig _config, List<Queue> queues)
        {
            Queues = queues;
            ConnectionFactory factory = new ConnectionFactory { HostName = _config.HostName };
            config = _config;


            IConnection connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: config.Exchange,
                type: "direct",
                durable: true);

            foreach (Queue queue in queues)
            {
                if (queue.CallbackMethod != null)
                {
                    channel.QueueDeclare(
                        queue: queue.Name,
                        durable: queue.Durable,
                        exclusive: queue.Exclusive,
                        autoDelete: queue.AutoDelete,
                        arguments: queue.Arguments);
                }

                channel.QueueBind(
                    queue: queue.Name,
                    exchange: config.Exchange,
                    routingKey: queue.Name);
            }

            

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                Message message = JsonConvert.DeserializeObject<Message>(json);

                Queue sourceQueue = Queues.Find(queue => queue.Name == ea.RoutingKey);

                if (sourceQueue.CallbackMethod == null)
                {
                    throw new ArgumentNullException("The callback method for a receiver queue cannot be null");
                }
                sourceQueue.CallbackMethod(this, message);
            };

            foreach(Queue queue in Queues)
            {
                channel.BasicConsume(
                    queue: queue.Name,
                    autoAck: queue.AutoAcknowledge,
                    consumer: consumer);
            }            
        }
    }
}
