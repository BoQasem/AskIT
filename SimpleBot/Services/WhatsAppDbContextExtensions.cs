using AskIT.Models;

namespace AskIT.Services
{
    public static class WhatsAppDbContextExtensions
    {
        public static void SaveMessage(
            this WhatsAppDbContext context,
            string userPhone,
            string messageText,
            string senderType,
            int? knowledgeBaseId,
            bool? isMatched,
            double? matchPercentage,
            bool needsHumanSupport)
        {
            var message = new Message
            {
                UserPhone = userPhone,
                MessageText = messageText,
                SenderType = senderType,
                KnowledgeBaseId = knowledgeBaseId,
                IsMatched = isMatched,
                MatchPercentage = matchPercentage,
                Timestamp = DateTime.UtcNow,
                NeedsHumanSupport = needsHumanSupport
            };

            context.Messages.Add(message);
            context.SaveChanges();
        }
    }
}
