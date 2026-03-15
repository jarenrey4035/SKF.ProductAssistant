using SKF.ProductAssistant.Models;

namespace SKF.ProductAssistant.State
{
    public static class ConversationService
    {
        private static ConversationState _state = new ConversationState();

        public static ConversationState GetState()
        {
            return _state;
        }

        public static void Update(string product, string attribute, string answer)
        {
            _state.LastProduct = product;
            _state.LastAttribute = attribute;
            _state.LastAnswer = answer;
        }
    }
}
