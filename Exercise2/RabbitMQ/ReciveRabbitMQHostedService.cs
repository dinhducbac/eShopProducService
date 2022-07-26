using eShopProductService.Services;
using Exercise2.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagerment.RabbitMQ
{
    public class ReciveRabbitMQHostedService : BackgroundService
    {
        public readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
       // private readonly DefaultObjectPool<IModel> _objectPool;
        public IServiceScopeFactory _serviceScopeFactory;
        private eShopDBContext _dbContext;
        public ReciveRabbitMQHostedService(IConfiguration configuration, /*IPooledObjectPolicy<IModel> objectPolicy,*/
            IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            //_objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
            _serviceScopeFactory = serviceScopeFactory;
            InitRabbitMQ();          
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.GetSection("RabbitMqConnection")["HostName"],
                UserName = _configuration.GetSection("RabbitMqConnection")["UserName"],
                Password = _configuration.GetSection("RabbitMqConnection")["Password"],
                Port = 5672,
                VirtualHost = "/",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            //_channel = _objectPool.Get();
            try
            {
                _channel.ExchangeDeclarePassive("changeAmount");

                QueueDeclareOk ok = _channel.QueueDeclarePassive("changeAmount");
                if (ok.MessageCount > 0)
                {
                    _channel.QueueBind("changeAmount", "changeAmount", "changeAmount", null);
                    _channel.BasicQos(0, 1, false);
                }
            }
            catch {}
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                if(ea.RoutingKey == "changeAmount")
                {
                    var content = System.Text.Encoding.UTF8.GetString(ea.Body);
                    var changeRequest = JsonConvert.DeserializeObject<ChangeAmountRequest>(content);
                    HandleMessage(content);
                    ChangeAmount(changeRequest.Id,changeRequest.Amount);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            //consumer.Shutdown += OnConsumerShutdown;
            //consumer.Registered += OnConsumerRegistered;
            //consumer.Unregistered += OnConsumerUnregistered;
            //consumer.ConsumerCancelled += OnConsumerConsumerCancelled;
            try
            {
                _channel.BasicConsume("changeAmount", false, consumer);
            }catch { }
            return Task.CompletedTask;
        }
        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        private void HandleMessage(string content)
        {
            Console.WriteLine(content);
        }
        public async Task ChangeAmount(int id, int amount)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<eShopDBContext>();
                _dbContext = dbContext;
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                product.Amount += amount;
                await _dbContext.SaveChangesAsync();
            }
            Console.WriteLine($"Da thay doi so luong sp co id = {id}");
        }
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
        public class ChangeAmountRequest 
        {
            public int Id { get; set; }
            public int Amount { get; set; }
        }
    }
}
