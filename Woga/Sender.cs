using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woga
{
    class Sender
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your name:");
            string name = Console.ReadLine();
            Send(string.Format("Hello my name is, {0}", name));
        }

        public static void Send(string smsbody)
        {
            var factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.HostName = "localhost";

            var conn = factory.CreateConnection();
            var channel = conn.CreateModel();
            channel.ExchangeDeclare("WogaExchange", ExchangeType.Direct);
            channel.QueueDeclare("WogaQueue", durable: true, autoDelete: false, exclusive: false, arguments: null);

            //channel.QueueDeclare("WogaQueue", false, false, false, null);

            channel.QueueBind("WogaQueue", "WogaExchange", "woga");

            var message = smsbody;
            var messagebody = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("WogaExchange", "woga", null, messagebody);
        }


    }
}
