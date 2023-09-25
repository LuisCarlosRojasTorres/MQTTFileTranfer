using Client.Model;
using MQTTnet;
using MQTTnet.Client;

namespace Client.TypeOfClients
{
    public static class ClientNoKeys
    {
        public static async Task Ping_Server( BrokerOptions brokerOptions )
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(brokerOptions.Ip, brokerOptions.Port).Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                await mqttClient.PingAsync(CancellationToken.None);

                if (mqttClient.IsConnected) { Console.WriteLine(">> Client connected!!"); }

                var mqttClientDisconnectOptions = mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
                await mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);
            }
        }

        public static async Task Connect_Client( BrokerOptions brokerOptions )
        {
            
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(brokerOptions.Ip, brokerOptions.Port).Build();

                // Setup message handling before connecting so that queued messages
                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine(">> Received application message.");
                    e.DumpToConsole();

                    return Task.CompletedTask;
                };                
               
                var connectResponse = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                //connectResponse.DumpToConsole();                

                if (mqttClient.IsConnected) { Console.WriteLine(">> Client connected!!"); }

                // Create the subscribe options including several topics with different options.
                // It is also possible to all of these topics using a dedicated call of _SubscribeAsync_ per topic.
                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(
                        f =>
                        {
                            f.WithTopic("HelloWorld");                            
                        })
                    .Build();

                var suscribedResponse = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
                
                // The response contains additional data sent by the server after subscribing.
                
                Console.WriteLine(">> Press enter to exit.");
                Console.ReadLine();

                var mqttClientDisconnectOptions = mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
                await mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);
                Console.WriteLine(">> Client Disconected.");
            }
        }


    }
}
