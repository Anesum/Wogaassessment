using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WogaConsumer
{
    class Consumer
    {
        static void Main(string[] args)
        {
            Receive();
        }
        public static void Receive()
        {
            var factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.HostName = "localhost";

            var conn = factory.CreateConnection();
            var channel = conn.CreateModel();

            channel.QueueDeclare("WogaQueue", durable: true, autoDelete: false, exclusive: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body);
                //Console.WriteLine("Message recieved:" + Encoding.UTF8.GetString(body));
                string[] name = body.Split(',');
                Console.WriteLine("Hello {0} I am your father!", name[1].Trim());
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queue: "WogaQueue", autoAck: false, consumer: consumer);
            Console.ReadLine();
        }
        }
}


