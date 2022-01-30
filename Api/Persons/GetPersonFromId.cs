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
using System.Collections.Generic;
using System.Linq;

namespace Api.Persons {
    public static class GetPersonFromId {
        [FunctionName("GetPersonFromId")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "person/id/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "gunshi-db", //Database id
                collectionName: "persons", //Container id
                ConnectionStringSetting = "CosmosDbConnectionString",
                SqlQuery = "select * from users r where r.id = {id}"
            )]IEnumerable<Person> persons,
            ILogger log) {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try {
                return new OkObjectResult(persons.First());
            } catch (Exception ex) {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
