using MQTTnet;
using MQTTnet.Client;

namespace PubSub
{
    public static class ClientNoKeys
    {
        public static async Task Publish_Application_Message(string BrokerAddress,int BrokerPort, string topic = "demo")
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(BrokerAddress, BrokerPort).WithCleanSession().Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("topic")
                    .WithPayload("LOBO")
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                if (mqttClient.IsConnected)
                {
                    Console.WriteLine("PubSub is Connected!");
                    Console.WriteLine("Press Enter to Publish.");
                    Console.ReadLine();
                    await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
                    Console.WriteLine("MQTT application message is published.");
                }
                else {
                    Console.WriteLine("PubSub is not Connected!");
                }
                Console.WriteLine("Press Enter to Disconnect.");
                Console.ReadLine();
                await mqttClient.DisconnectAsync();

                
            }
        }
    }
}
