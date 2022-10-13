﻿using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerShark.Core.Poker;

namespace PokerShark.Core.RPC
{
    public class Server
    {
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer consumer;

        public Server(string hostname)
        {
            Log.Information("Initializing rpc server.");
            // init channel
            factory = new ConnectionFactory() { HostName = hostname };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            // Setup RPC queue
            channel.QueueDeclare(queue: "rpc_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(0, 1, false);

            // Setup consumer
            consumer = new Consumer(channel, new Bot());
            channel.BasicConsume(queue: "rpc_queue",
                  autoAck: false, consumer: consumer);
        }

        ~Server()
        {
            if (connection != null)
                connection.Close();
        }
    }
}
