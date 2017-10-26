﻿namespace WebServer
{
    using Contracts;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class WebServer : IRunnable
    {
        private const string localHostIpAddress = "127.0.0.1";

        private readonly int port;
        private readonly IHandleable mvcRequestHandler;
        private readonly IHandleable resourceHandler;
        private readonly TcpListener listener;
        private bool isRunning;

        public WebServer(int port, IHandleable mvcRequestHandler, IHandleable resourceHandler)
        {
            this.port = port;
            this.listener = new TcpListener(IPAddress.Parse(localHostIpAddress), port);

            this.mvcRequestHandler = mvcRequestHandler;
            this.resourceHandler = resourceHandler;
        }

        public void Run()
        {
            this.listener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server running on {localHostIpAddress}:{this.port}");

            Task.Run(this.ListenLoop).Wait();
        }

        private async Task ListenLoop()
        {
            while (this.isRunning)
            {
                var client = await this.listener.AcceptSocketAsync();
                var connectionHandler = new ConnectionHandler(client, this.mvcRequestHandler, this.resourceHandler);
                await connectionHandler.ProcessRequestAsync();
            }
        }
    }
}
