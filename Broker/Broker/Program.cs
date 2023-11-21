using Broker.Model;
using Broker.TypesOfBrokers;
using System.Text.Json;

BrokerOptions brokerOptions;

using (StreamReader file = File.OpenText(Path.Combine("BrokerConfig.json")))
{
    brokerOptions = JsonSerializer.Deserialize<BrokerOptions>(file.ReadToEnd());
    Console.WriteLine($" BROKER");
    Console.WriteLine($" - Broker running at: {BrokerNoKeys.GetLocalIPAddress()}");
    Console.WriteLine($" - Broker Port: {brokerOptions.Port}");
}

await BrokerNoKeys.Run_Minimal_Server(brokerOptions);



