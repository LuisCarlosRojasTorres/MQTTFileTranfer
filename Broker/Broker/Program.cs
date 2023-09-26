using Newtonsoft.Json;
using Broker.Model;
using Broker.TypesOfBrokers;

BrokerOptions brokerOptions;

using (StreamReader file = File.OpenText(Path.Combine("BrokerConfig.json")))
{
    brokerOptions = JsonConvert.DeserializeObject<BrokerOptions>(file.ReadToEnd());
}

Console.WriteLine($" BROKER");
Console.WriteLine($" - Broker running at: {BrokerNoKeys.GetLocalIPAddress()}");
Console.WriteLine($" - Broker Port: {brokerOptions.Port}");

byte[] dummyFile = BrokerNoKeys.FileToByteArrayConverter(brokerOptions.FileToTransfer);
await BrokerNoKeys.Run_Minimal_Server(brokerOptions, "HelloWorld", dummyFile);



