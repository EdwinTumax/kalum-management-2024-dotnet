using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using KalumManagement.Dtos;
using RabbitMQ.Client;

namespace KalumManagement.Services
{
    public class KalumQueueService : IKalumQueueService
    {
        public readonly ILogger<KalumQueueService> Logger;
        public readonly IConfiguration Configuration;
        public KalumQueueService(ILogger<KalumQueueService> _logger, IConfiguration _configuration)
        {
            this.Logger = _logger;
            this.Configuration = _configuration;
        }
        public async Task<bool> CandidateCreateOrderAsync(AspiranteCreateOrderDTO order)
        {
            bool response = false;
            ConnectionFactory connectionFactory = new ConnectionFactory();
            IConnection connection = null;
            IModel channel = null;
            connectionFactory.HostName = this.Configuration.GetValue<string>("RabbitConfig:Host");
            connectionFactory.VirtualHost = this.Configuration.GetValue<string>("RabbitConfig:VirtualHost");
            connectionFactory.Port = this.Configuration.GetValue<int>("RabbitConfig:Port");
            connectionFactory.UserName = this.Configuration.GetValue<string>("RabbitConfig:User");            
            connectionFactory.Password = this.Configuration.GetValue<string>("RabbitConfig:Password");
            try
            {
                connection = connectionFactory.CreateConnection();
                channel = connection.CreateModel();
                channel.BasicPublish(this.Configuration.GetValue<string>("RabbitConfig:Exchange"),this.Configuration.GetValue<string>("RabbitConfig:RouteKey"),null,Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order)));
                response = true;
                await Task.Delay(100);
            }
            catch(Exception e)
            { 
                this.Logger.LogError(e.Message);
            }
            finally
            {
                channel.Close();
                connection.Close();
            }

            return response;
        }
    }
}