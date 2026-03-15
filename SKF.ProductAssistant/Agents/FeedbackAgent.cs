using SKF.ProductAssistant.Services;
using SKF.ProductAssistant.State;

namespace SKF.ProductAssistant.Agents
{
    public class FeedbackAgent
    {
        private  readonly RedisService _redisService;
       // private readonly ConversationService _conversationService;

        public FeedbackAgent()
        {
            _redisService = new RedisService();
         //   _conversationService = new ConversationService();
        }

        public async Task<string> SaveFeedback(string message)
        {
            var state = ConversationService.GetState();

            var product = state.LastProduct;
            var attribute = state.LastAttribute;

            if (string.IsNullOrEmpty(product))
            {
                return "No previous product found to attach feedback.";
            }

            var key = $"feedback:{product}:{attribute}";

            await _redisService.SetAsync(key, message);

            return $"Thanks—your feedback for {product} / {attribute} has been saved.";
        }
    }
}