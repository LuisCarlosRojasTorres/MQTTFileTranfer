// See https://aka.ms/new-console-template for more information
using PubSub;
using Newtonsoft.Json;
using Client.Model;

BrokerOptions brokerOptions;

using (StreamReader file = File.OpenText(Path.Combine("BrokerConfig.json")))
{
    brokerOptions = JsonConvert.DeserializeObject<BrokerOptions>(file.ReadToEnd());
}

Console.WriteLine($" PUBLISHER");
Console.WriteLine($" - Broker running at: {brokerOptions.Ip}");
Console.WriteLine($" - Broker Port: {brokerOptions.Port}");

await ClientNoKeys.Publish_Application_Message(brokerOptions);



byte[] dummyFile = ClientNoKeys.FileToByteArrayConverter(brokerOptions.FileToTransfer);