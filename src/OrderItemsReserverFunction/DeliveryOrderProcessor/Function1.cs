using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DeliveryOrderProcessor
{
    public static class Function1
    {
        [FunctionName("DeliveryOrderProcessor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "order/{id}")] HttpRequest req,
            [CosmosDB(databaseName: "Delivery", collectionName: "Orders", ConnectionStringSetting = "CosmosDbConnectionString")] IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string id = req.Query["id"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data != null)
            {
                await documentsOut.AddAsync(new
                {
                    id = Guid.NewGuid().ToString(),
                    order = data
                });
            }

            string responseMessage = string.IsNullOrEmpty(requestBody)
                ? "Put request body with order"
                : $"This HTTP triggered function executed successfully.";
           
            return new OkObjectResult(responseMessage);
        }
    }
}
