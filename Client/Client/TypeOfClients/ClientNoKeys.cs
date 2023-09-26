using Client.Model;
using MQTTnet;
using MQTTnet.Client;
using System.Text;

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

        public static async Task Suscribe_Application_Message( BrokerOptions brokerOptions, string topic="demo")
        {
            
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(brokerOptions.Ip, brokerOptions.Port).Build();
                //var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("10.0.0.35").Build();

                // Setup message handling before connecting so that queued messages
                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine(">> Received application message!!.");
                    Console.WriteLine($">> MessageDecoded: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                    
                    return Task.CompletedTask;
                };                
               
                var connectResponse = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                connectResponse.DumpToConsole();                

                if (mqttClient.IsConnected) 
                { 
                    Console.WriteLine(">> Client connected!!"); 
                }
                else
                {
                    Console.WriteLine(">> Client NOT connected!!");
                }

                // Create the subscribe options including several topics with different options.
                // It is also possible to all of these topics using a dedicated call of _SubscribeAsync_ per topic.
                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(
                        f =>
                        {
                            f.WithTopic(topic);                            
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
