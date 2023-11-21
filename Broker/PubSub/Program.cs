// See https://aka.ms/new-console-template for more information
using PubSub;
using Client.Model;
using System.Text.Json;

BrokerOptions brokerOptions;

using (StreamReader file = File.OpenText(Path.Combine("BrokerConfig.json")))
{
    brokerOptions = JsonSerializer.Deserialize<BrokerOptions>(file.ReadToEnd());
}

Console.WriteLine($" PUBLISHER");
Console.WriteLine($" - Trying to connect to Broker at: {brokerOptions.Ip}");
Console.WriteLine($" - Trying to connect to Broker Port: {brokerOptions.Port}");
Console.WriteLine($" - Topic: {brokerOptions.Topic}");


await ClientNoKeys.Publish_File(brokerOptions);
//await ClientNoKeys.Publish_Directory(brokerOptions);