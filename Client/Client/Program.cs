using Client.Model;
using Client.TypeOfClients;
using System.Text.Json;

BrokerOptions brokerOptions;

using (StreamReader file = File.OpenText(Path.Combine("BrokerConfig.json")))
{
    brokerOptions = JsonSerializer.Deserialize<BrokerOptions>(file.ReadToEnd());
}

Console.WriteLine($" SUBSCRIBER");
Console.WriteLine($" - Trying to connect to Broker at: {brokerOptions.Ip}");
Console.WriteLine($" - Trying to connect to Broker Port: {brokerOptions.Port}");
Console.WriteLine($" - Topic: {brokerOptions.Topic}");

await ClientNoKeys.Suscribe_File(brokerOptions);

