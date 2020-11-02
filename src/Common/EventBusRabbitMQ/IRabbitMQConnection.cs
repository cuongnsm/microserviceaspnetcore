using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace EventBusRabbitMQ 
{
    public interface IRabbitMQConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
