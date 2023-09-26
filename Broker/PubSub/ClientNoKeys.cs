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

        public static async Task Publish_File(BrokerOptions brokerOptions)
        {
            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(brokerOptions.Ip, brokerOptions.Port).WithCleanSession().Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                

                if (mqttClient.IsConnected)
                {
                    Console.WriteLine("PubSub is Connected!");
                    Console.WriteLine("Press Enter to Publish.");
                    Console.ReadLine();

                    byte[] fileConvertedIntoByteArray = FileToByteArrayConverter(brokerOptions.FileToTransfer);
                    var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("filename")
                    .WithPayload(brokerOptions.FileToTransfer)
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();
                    
                    await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
                    Console.WriteLine("MQTT application message is published.");

                    applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("fileByteArray")
                    .WithPayload(fileConvertedIntoByteArray)
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                    await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
                }
                else
                {
                    Console.WriteLine("PubSub is not Connected!");
                }
                Console.WriteLine("Press Enter to Disconnect.");
                Console.ReadLine();
                await mqttClient.DisconnectAsync();


            }
        }
    }
}
