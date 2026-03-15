using Microsoft.SemanticKernel;
using SKF.ProductAssistant.Services;
using SKF.ProductAssistant.Agents;

namespace SKF.ProductAssistant.Orchestrator
{
    public class IntentRouter
    {
        private readonly Kernel _kernel;

        public IntentRouter()
        {
            _kernel = KernelService.CreateKernel();
        }

        public async Task<string> RouteAsync(string message)
        {
            var prompt = $"""
You are an intent classifier.

Classify message into:
question
feedback

Return only one word.

Message: {message}
""";

            var result = await _kernel.InvokePromptAsync(prompt);

            var intent = result.ToString().Trim().ToLower();

            if (intent.Contains("question"))
            {
                var qaAgent = new QAAgent();
                return await qaAgent.ExtractProductAndAttribute(message);
            }

            if (intent.Contains("feedback"))
            {
                var feedbackAgent = new FeedbackAgent();
                return await feedbackAgent.SaveFeedback(message);
            }

            return "Feedback detected (agent coming next step)";
        }
    }
}