using Client.Model;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;
using PubSub.Model;
using System.IO;
using System.Text;

namespace Client.TypeOfClients
{
    public static class ClientNoKeys
    {
        public static async Task Suscribe_File(BrokerOptions brokerOptions)
        {
            string SFTFileContent = "";

            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                CancellationTokenSource timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(brokerOptions.ConnectionTimeout));
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(brokerOptions.Ip, brokerOptions.Port).Build();

                // >> Begin start event
                /*
                mqttClient.DisconnectedAsync += async e =>
                {
                    if (e.ClientWasConnected)
                    {
                        // Use the current options as the new options.
                        Console.WriteLine(" >> Broker Disconnected");                        
                    }
                };
                */
                // >> End start event



                // Setup message handling before connecting so that queued messages
                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    if (e.ApplicationMessage.Topic == brokerOptions.Topic)
                    {
                        SFTFileContent = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        Console.WriteLine($">> Received SFTFile");
                        ConvertJsonToFile(SFTFileContent);                        
                    }                    
                    return Task.CompletedTask;
                };
                
                //var connectResponse = await mqttClient.ConnectAsync(mqttClientOptions, timeoutToken.Token);

                // > TryPing
                while (true)
                {
                    try
                    {
                        // This code will also do the very first connect! So no call to _ConnectAsync_ is required in the first place.
                        if (!await mqttClient.TryPingAsync())
                        {
                            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                            // Subscribe to topics when session is clean etc.
                            Console.WriteLine("The MQTT client is connected.");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("ReConnection failed");
                    }                    
                }
                // >> End TryPing

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
                            f.WithTopic(brokerOptions.Topic);
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

        private static void ConvertJsonToFile(string SFTFileContent)
        {
            SftFile sftFile = JsonSerializer.Deserialize<SftFile>(SFTFileContent);
            Console.WriteLine($">> Converted SFTFile");

            try 
            {
                File.WriteAllBytes(sftFile.FileName, sftFile.Payload);
                Console.WriteLine($">> SFTFile created");
                VerifyChecksum(sftFile);
                Console.WriteLine($">> SFTFile checksum verified");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            
        }

        private static string ConvertFileToJson(string filePath)
        {
            SftFile sftFile = new SftFile(filePath);
            string sftFileSerialized = JsonSerializer.Serialize(sftFile);
            return sftFileSerialized;
        }

        private static void VerifyChecksum(SftFile sftFile)
        { 
            if( sftFile != null ) 
            {
                if (sftFile.HashMd5 == sftFile.CalculateHashMd5(sftFile.FileName)) 
                {
                    Console.WriteLine("MD5 correct!!!");
                }
            
            }
        }

    }
}
