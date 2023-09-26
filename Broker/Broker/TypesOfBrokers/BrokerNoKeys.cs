using System.Net.Sockets;
using System.Net;
using MQTTnet.Server;
using MQTTnet;

using Broker.Model;

namespace Broker.TypesOfBrokers
{
    public static class BrokerNoKeys
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {                    
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static async Task Run_Minimal_Server(BrokerOptions brokerOptions, string topic, byte[] payload)
        {
            //This sample starts a simple MQTT server which will accept any TCP connection.
            var mqttFactory = new MqttFactory();

            var mqttServerOptions = mqttFactory.CreateServerOptionsBuilder().WithDefaultEndpoint().Build();


            using (var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions))
            {
                await mqttServer.StartAsync();
                
                // Stop and dispose the MQTT server if it is no longer needed!
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();

                await mqttServer.StopAsync();
            }
        }

        public static byte[] FileToByteArrayConverter(string filePath)
        {
            byte[] fileToTransfer = File.ReadAllBytes(filePath);

            return fileToTransfer;
        }
    }
}
