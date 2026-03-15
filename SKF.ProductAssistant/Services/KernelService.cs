using Microsoft.SemanticKernel;

namespace SKF.ProductAssistant.Services
{
    public class KernelService
    {
        public static Kernel CreateKernel()
        {
            var builder = Kernel.CreateBuilder();

            var endpoint = Environment.GetEnvironmentVariable("AOAI_ENDPOINT");
            var apiKey = Environment.GetEnvironmentVariable("AOAI_API_KEY");
            var deployment = Environment.GetEnvironmentVariable("AOAI_DEPLOYMENT");

            builder.AddAzureOpenAIChatCompletion(
                deploymentName: deployment,
                endpoint: endpoint,
                apiKey: apiKey
            );

            return builder.Build();
        }
    }
}