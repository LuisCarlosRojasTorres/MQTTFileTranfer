using MQTTnet;
using MQTTnet.Client;
using Client.Model;

namespace PubSub
{
    public static class ClientNoKeys
    {
        public static async Task Publish_Application_Message(BrokerOptions brokerOptions, string topic = "demo")
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(brokerOptions.Ip, brokerOptions.Port).WithCleanSession().Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
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

        public static byte[] FileToByteArrayConverter(string filePath)
        {
            byte[] fileToTransfer = File.ReadAllBytes(filePath);

            return fileToTransfer;
        }

    }
}
