using System;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Team5_factory
{
    class Program

    {
        private static EventHubClient eventHubClient;
private const string EventHubConnectionString = "Endpoint=sb://ehteam5.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=D1b791KiftEtkWLUiFiqULtbYz8/SQAYzudyiKjXWlU=";
private const string EventHubName = "ehteam5eh1";
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }
        private static async Task MainAsync(string[] args)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but this simple scenario
            // uses the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
            {
                EntityPath = EventHubName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(100);

            await eventHubClient.CloseAsync();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
        // Creates an event hub client and sends 100 messages to the event hub.
        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    Random r = new Random();
                    int rInt = r.Next(0, 100); //for ints
                    int range = 100;
                    double rDouble = r.NextDouble() * range; //for doubles
                    JObject signals = new JObject();

                    Random r2 = new Random();
                    int rInt2 = r.Next(-100, 100); //for ints
                    int range2 = 100;
                    double rDouble2 = r2.NextDouble() * range2; //for doubles
                    Guid id = Guid.NewGuid();

                    signals.Add("a1", rInt);
                    signals.Add("b1", rDouble);
                    signals.Add("c1", rInt2);
                    signals.Add("d1", rDouble2);
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    JObject jo = new JObject();
                    jo.Add("DeviceId", id);
                    jo.Add("msgType", "signals");
                    jo.Add("timestamp", unixTimestamp);
                    jo.Add("data", signals);

                    Console.WriteLine($"Sending message: {jo.ToString()}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(jo.ToString())));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10000);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}
