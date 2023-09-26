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

Console.WriteLine($" CLIENTE");
//await ClientNoKeys.Ping_Server(brokerOptions);

ClientNoKeys.Suscribe_Application_Message(brokerOptions);
//await ClientNoKeys.Publish_Application_Message( brokerOptions );

Console.WriteLine("Press Enter to exit.");
Console.ReadLine();