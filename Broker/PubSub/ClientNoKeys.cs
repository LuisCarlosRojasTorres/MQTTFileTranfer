using MQTTnet;
using MQTTnet.Client;
using Client.Model;
using PubSub.Model;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace PubSub
{
    public static class ClientNoKeys
    {
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

                CancellationTokenSource timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(brokerOptions.ConnectionTimeout));
                await mqttClient.ConnectAsync(mqttClientOptions, timeoutToken.Token);                

                if (mqttClient.IsConnected)
                {
                    Console.WriteLine("PubSub is Connected!");
                    Console.WriteLine("Press Enter to Publish.");
                    Console.ReadLine();

                    string sftFileSerialized  = ConvertFileToJson(brokerOptions.FileToTransfer);

                    var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(brokerOptions.Topic)
                    .WithPayload(sftFileSerialized)
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();
                    
                    await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
                    Console.WriteLine("MQTT application message is published.");
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

        private static string ConvertFileToJson(string filePath)
        {
            SftFile sftFile = new SftFile(filePath);
            string sftFileSerialized = JsonSerializer.Serialize(sftFile);
            return sftFileSerialized;
        }

        public static async Task Publish_Directory(BrokerOptions brokerOptions)
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

                    string[] arrayOfFiles = Directory.GetFiles(brokerOptions.DirectoryToTransfer);
                    foreach (string fileToTransfer in arrayOfFiles)
                    {
                        string sftFileSerialized = ConvertFileToJson(fileToTransfer);

                        var applicationMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(brokerOptions.Topic)
                        .WithPayload(sftFileSerialized)
                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                        .Build();

                        await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
                        Console.WriteLine($"File {fileToTransfer} is published.");
                    }
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
