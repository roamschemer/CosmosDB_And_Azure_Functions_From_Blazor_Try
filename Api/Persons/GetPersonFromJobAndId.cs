using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Data;

namespace Api.Persons {
    public static class GetPersonFromJobAndId {
        [FunctionName("GetPersonFromJobAndId")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "person/{job}/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "gunshi-db", //Database id
                collectionName: "persons", //Container id
                ConnectionStringSetting = "CosmosDbConnectionString",
                Id = "{id}",
                PartitionKey = "{job}")] Person person,
            ILogger log) {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try {
                return new OkObjectResult(person);
            } catch (Exception ex) {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
