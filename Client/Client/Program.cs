using Client.Model;
using Client.TypeOfClients;
using Newtonsoft.Json;

BrokerOptions brokerOptions;

using (StreamReader file = File.OpenText(Path.Combine("BrokerConfig.json")))
{
    brokerOptions = JsonConvert.DeserializeObject<BrokerOptions>(file.ReadToEnd());
    Console.WriteLine($" SUBSCRIBER");
    Console.WriteLine($" - Broker IP Adress: {brokerOptions.Ip}");
    Console.WriteLine($" - Broker Port: {brokerOptions.Port}");
}

await ClientNoKeys.Suscribe_File(brokerOptions);

