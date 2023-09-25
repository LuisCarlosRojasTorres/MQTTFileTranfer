// See https://aka.ms/new-console-template for more information

using Client.Model;
using Client.TypeOfClients;
using Newtonsoft.Json;

BrokerOptions brokerOptions;

using (StreamReader file = File.OpenText(Path.Combine("BrokerConfig.json")))
{
    brokerOptions = JsonConvert.DeserializeObject<BrokerOptions>(file.ReadToEnd());
    Console.WriteLine($" - Broker IP Adress: {brokerOptions.Ip}");
    Console.WriteLine($" - Broker Port: {brokerOptions.Port}");
}

//await ClientNoKeys.Ping_Server(brokerOptions);

await ClientNoKeys.Connect_Client(brokerOptions);

Console.WriteLine("Press Enter to exit.");
Console.ReadLine();