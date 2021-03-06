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
using System.Linq;

namespace Api.Persons {
    public static class UpdatePerson {
        [FunctionName("UpdatePerson")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "person/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "gunshi-db", //Database id
                collectionName: "persons", //Container id
                ConnectionStringSetting = "CosmosDbConnectionString")]DocumentClient client,
            string id,
            ILogger log) {
            var databaseName = "gunshi-db";
            var collectionName = "persons";
            try {
                string requestBody;
                using (StreamReader streamReader = new StreamReader(req.Body)) {
                    requestBody = await streamReader.ReadToEndAsync();
                }
                var person = JsonConvert.DeserializeObject<Person>(requestBody);
                var option = new FeedOptions { EnableCrossPartitionQuery = true };
                var collectionUri = UriFactory.CreateDocumentCollectionUri(databaseName, collectionName);
                var document = client.CreateDocumentQuery(collectionUri, option).Where(t => t.Id == id).AsEnumerable().FirstOrDefault();
                if (document == null) {
                    return new NotFoundResult();
                }
                document.SetPropertyValue("name", person.Name);
                document.SetPropertyValue("attributes", person.Attributes);
                document.SetPropertyValue("items", person.Items);
                await client.ReplaceDocumentAsync(document);
            } catch (Exception ex) {
                return new BadRequestObjectResult(ex);
            }
            return new OkResult();
        }
    }
}
