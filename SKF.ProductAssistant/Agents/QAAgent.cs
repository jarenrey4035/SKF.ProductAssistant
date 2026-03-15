using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using SKF.ProductAssistant.Models;
using SKF.ProductAssistant.Services;
using SKF.ProductAssistant.State;

namespace SKF.ProductAssistant.Agents
{
    public class QAAgent
    {
        private readonly Kernel _kernel;
        private readonly JsonDataService _dataService;
        //  private readonly ConversationService _conversationService;
        private readonly RedisService _redisService;

        public QAAgent()
        {
            _kernel = KernelService.CreateKernel();
            _dataService = new JsonDataService();
            //    _conversationService = new ConversationService();
            _redisService = new RedisService();
        }

        public async Task<string> ExtractProductAndAttribute(string message)
        {
            var prompt = @"Extract the product designation and attribute from the user question.

Return JSON format only.

Example:
User: What is the width of 6205?
Output:
{""product"":""6205"",""attribute"":""width""}

User: " + message;

            var result = await _kernel.InvokePromptAsync(prompt);

            var query = JsonConvert.DeserializeObject<ProductQuery>(result.ToString());

            if (query == null)
                return "Sorry, I couldn't understand the request.";

            var state = ConversationService.GetState();

            // Handle follow-up questions
            if (string.IsNullOrEmpty(query.product))
            {
                query.product = state.LastProduct;
            }

            if (string.IsNullOrEmpty(query.attribute))
            {
                query.attribute = state.LastAttribute;
            }

            if (string.IsNullOrEmpty(query.product))
            {
                return "Please specify a product designation.";
            }

            // Redis Cache Key
            var cacheKey = $"{query.product}:{query.attribute}".ToLower();

            // Check Redis Cache
            var cached = await _redisService.GetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
            {
                //  Update Conversation Memory
                ConversationService.Update(query.product, query.attribute, cached);

                return cached;
            }

            // 2️⃣ Read JSON Datasheet
            var value = _dataService.GetAttribute(query.product, query.attribute);

            if (value == null)
            {
                return $"Sorry, I can’t find that information for '{query.product}'.";
            }

            var answer = $"The {query.attribute} of the {query.product} bearing is {value}.";

            // 3️⃣ Save to Redis Cache
            await _redisService.SetAsync(cacheKey, answer);

            // 4️⃣ Update Conversation Memory
            ConversationService.Update(query.product, query.attribute, answer);

            return answer;
        }
    }
}