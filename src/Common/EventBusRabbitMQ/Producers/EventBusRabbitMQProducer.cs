using System.ComponentModel.DataAnnotations;
using System.Text;
using EventBusRabbitMQ.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EventBusRabbitMQ.Producers
{
    public class EventBusRabbitMQProducer
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;

        public EventBusRabbitMQProducer(IRabbitMQConnection rabbitMQConnection)
        {
            _rabbitMQConnection = rabbitMQConnection;
        }

        public void PublishBaskCheckout(string queueName, BasketCheckoutEvent publishModel)
        {
            using (var channel = _rabbitMQConnection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(publishModel);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties basicProperties = channel.CreateBasicProperties();
                basicProperties.Persistent = true;
                basicProperties.DeliveryMode = 2;
                channel.BasicAcks += (sender, eventArgs) =>
                {
                    System.Console.WriteLine("Sent to RabbitMQ");
                };
                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties, body);
                channel.WaitForConfirmsOrDie();
            }        
        }
    }
}
