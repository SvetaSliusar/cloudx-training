using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace OrderItemsReserverAzure
{
    public static class Function1
    {
        [FunctionName("OrderItemsReserver")]
        public async static Task Run(
            [ServiceBusTrigger("orders", Connection = "ServiceBusConnectionString")]string myQueueItem, 
            [Blob("outcontainer/{rand-guid}", FileAccess.Write, Connection = "CloudxConnectionString")] Stream result,
            ILogger log)
        {
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                await result.WriteAsync(Encoding.UTF8.GetBytes(myQueueItem), 0, myQueueItem.Length);
            }
            catch(Exception e)
            {
                log.LogError(e.Message);
            }
        }
    }
}
