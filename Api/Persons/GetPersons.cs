using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using Data;
using Microsoft.Azure.Documents.Linq;
using System.Linq;
using System.Collections.Generic;

namespace Api.Persons {
    public static class GetPersons {
        [FunctionName("GetPersons")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "person")] HttpRequest req,
            [CosmosDB(
                databaseName: "gunshi-db", //Database id
                collectionName: "persons", //Container id
                ConnectionStringSetting = "CosmosDbConnectionString")]DocumentClient client,
            ILogger log) {
            var databaseName = "gunshi-db"; //Database id
            var collectionName = "persons"; //Container id
            log.LogInformation("C# HTTP trigger function processed a request.");

            var names = req.Query["name"];
            var jobs = req.Query["job"];
            var attributes = req.Query["attribute"];

            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(databaseName, collectionName);
            var query = client.CreateDocumentQuery<Person>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                .Where(p => string.IsNullOrWhiteSpace(names) || p.Name.Contains(names))
                .Where(p => string.IsNullOrWhiteSpace(jobs) || p.Job.Contains(jobs))
                .Where(p => string.IsNullOrWhiteSpace(attributes) || p.Attributes.Contains(attributes))
                .AsDocumentQuery();

            var persons = new List<Person>();
            while (query.HasMoreResults) {
                foreach (Person result in await query.ExecuteNextAsync()) {
                    persons.Add(result);
                }
            }

            return new OkObjectResult(persons);
        }
    }
}
