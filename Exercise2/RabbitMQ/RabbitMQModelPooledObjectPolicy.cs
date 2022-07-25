using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;

namespace EmployeeManagerment.RabbitMQ
{
    public class RabbitMQModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitMQOption _options;

        private readonly IConnection _connection;
        public RabbitMQModelPooledObjectPolicy(IOptions<RabbitMQOption> optionAccs)
        {
            _options = optionAccs.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                VirtualHost = _options.VHost,
            };

            return factory.CreateConnection();
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
