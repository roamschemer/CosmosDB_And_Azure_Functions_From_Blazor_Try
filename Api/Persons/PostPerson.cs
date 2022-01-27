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
    public static class PostPerson {
        [FunctionName("PostPerson")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "person")] HttpRequest req,
            [CosmosDB(
                databaseName: "gunshi-db", //Database id
                collectionName: "persons", //Container id
                ConnectionStringSetting = "CosmosDbConnectionString")]
            IAsyncCollector<Person> documentsOut,
            ILogger log) {

            try {
                string requestBody;
                using (StreamReader streamReader = new StreamReader(req.Body)) {
                    requestBody = await streamReader.ReadToEndAsync();
                }
                var person = JsonConvert.DeserializeObject<Person>(requestBody);
                person.Id = Guid.NewGuid();
                await documentsOut.AddAsync(person);
                return new OkObjectResult(person);
            } catch (Exception ex) {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
