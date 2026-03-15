using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SKF.ProductAssistant.Orchestrator;
using System.Net;

namespace SKF.ProductAssistant
{
    public class ProductAssistantFunction
    {
        [Function("AskProduct")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var message = await new StreamReader(req.Body).ReadToEndAsync();

            var router = new IntentRouter();

            var intent = await router.RouteAsync(message);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Detected intent: {intent}");

            return response;
        }
    }
}