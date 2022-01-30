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
using System.Linq;

namespace Api.Persons {
    public static class DeletePerson {
        [FunctionName("DeletePerson")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "person/{job}/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "gunshi-db", //Database id
                collectionName: "persons", //Container id
                ConnectionStringSetting = "CosmosDbConnectionString")]DocumentClient client,
            string job,
            string id,
            ILogger log) {
            var databaseName = "gunshi-db";
            var collectionName = "persons";
            try {
                var option = new FeedOptions { EnableCrossPartitionQuery = true };
                var collectionUri = UriFactory.CreateDocumentCollectionUri(databaseName, collectionName);

                var document = client.CreateDocumentQuery(collectionUri, option).Where(t => t.Id == id)
                      .AsEnumerable().FirstOrDefault();

                if (document == null) {
                    return new NotFoundResult();
                }
                await client.DeleteDocumentAsync(document.SelfLink, new RequestOptions {
                    PartitionKey = new Microsoft.Azure.Documents.PartitionKey(job)
                });

                return new OkResult();
            } catch (Exception ex) {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
