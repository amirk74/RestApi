

using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestApi.Model;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RestApi
{
    //[RequestHttpHeader("Idempotency-Key", isRequired: false)]
    //[RequestHttpHeader("Authorization", isRequired: true)]
    public static class Function1
    {


                [FunctionName("AddEntity")]
                [ProducesResponseType(typeof(MyEntity), (int)HttpStatusCode.OK)]
                public static async Task<IActionResult> AddEntity(
                [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
                [RequestBodyType(typeof(MyEntity), "request")] HttpRequest req, ILogger log)
                {

                    log.LogInformation("C# HTTP trigger AddEntity function processed a request.");
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    MyEntity data = JsonConvert.DeserializeObject<MyEntity>(requestBody);
                    return new OkObjectResult(Tasks.AddEntity(data));
                }

                [FunctionName("GetEntities")]
                [ProducesResponseType(typeof(MyEntity), (int)HttpStatusCode.OK)]
                public static async Task<IActionResult> GetEntities(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
                [RequestBodyType(typeof(MyEntity), "request")] HttpRequest req, ILogger log)
                {
                    log.LogInformation("C# HTTP trigger GetEntities function processed a request.");
                    return new OkObjectResult(Tasks.RetrieveAllEntities());
                }

                [FunctionName("GetEntityByID")]
                [ProducesResponseType(typeof(MyEntity), (int)HttpStatusCode.OK)]
                public static async Task<IActionResult> GetEntityByID(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetEntity/{id}")]
                [RequestBodyType(typeof(MyEntity), "request")] HttpRequest req, ILogger log,string id)
                {
                    log.LogInformation("C# HTTP trigger GetEntityByID function processed a request.");
                    return new OkObjectResult(Tasks.RetrieveEntity(id));
                }



                [FunctionName("UpdateEntity")]
                [ProducesResponseType(typeof(MyEntity), (int)HttpStatusCode.OK)]
                public static async Task<IActionResult> UpdateEntity(
                [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Update/{id}")]
                [RequestBodyType(typeof(MyEntity), "request")] HttpRequest req, ILogger log, string id)
                {
                    log.LogInformation("C# HTTP trigger UpdateEntity function processed a request.");
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    MyEntity data = JsonConvert.DeserializeObject<MyEntity>(requestBody);
                    return new OkObjectResult(Tasks.UpdateEntity(data,id));
                }


                [FunctionName("DeleteEntity")]
                [ProducesResponseType(typeof(MyEntity), (int)HttpStatusCode.OK)]
                public static async Task<IActionResult> DeleteEntity(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Delete/{id}")]
                [RequestBodyType(typeof(MyEntity), "request")] HttpRequest req, ILogger log, string id)
                {
                    log.LogInformation("C# HTTP trigger DeleteEntity function processed a request.");
                    Tasks.DeleteEntity(id);
                    return new OkObjectResult("Record deleted");
                }

            }
    }
