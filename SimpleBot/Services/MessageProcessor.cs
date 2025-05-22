using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace AskIT.Services
{
    public class MessageProcessor 
    {
        private readonly MessageMatcher _matcher;
        private readonly MessageSender _sender;
        private readonly MessageLogger _logger;
        private readonly IConfiguration _configuration;

        public MessageProcessor(
            MessageMatcher matcher,
            MessageSender sender,
            MessageLogger logger,
            IConfiguration configuration)
        {
            _matcher = matcher;
            _sender = sender;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task ProcessIncomingMessageAsync(string senderPhoneNumber, string incomingMessageText, string senderId)
        {
            string header = _configuration["BotSettings:Header"];
            string footer = _configuration["BotSettings:Footer"];
            string directToITMessage = _configuration["BotSettings:DirectToITMessage"];
            string noAnswerMessage = _configuration["BotSettings:NoAnswerMessage"];
            double threshold = double.Parse(_configuration["BotSettings:GlobalPercentage"]);

            string responseText;
            int? kbId = null;
            bool? isMatched = false;
            double? matchPercent = 0;

            if (incomingMessageText.Trim() == "0")
            {
                responseText = $"{header}{directToITMessage}";
                var lastMsg = _logger.GetLastUserMessage(senderPhoneNumber);
                if (lastMsg != null)
                {
                    lastMsg.IsMatched = false;
                    lastMsg.NeedsHumanSupport = true;
                    _logger.SaveChanges();
                }
            }
            else
            {
                var matchResult = _matcher.FindBestMatch(incomingMessageText);
                if (matchResult != null && matchResult.MatchPercent > threshold)
                {
                    kbId = matchResult.KnowledgeBaseId;
                    isMatched = true;
                    responseText = $"{header}{matchResult.IdealAnswer}{footer}";
                }
                else
                {
                    responseText = $"{header}{noAnswerMessage}";
                }
                matchPercent = matchResult?.MatchPercent ?? 0;
                _logger.LogMessage(senderPhoneNumber, incomingMessageText, "User", kbId, isMatched, matchPercent, false);
            }

            JObject responseObj = new JObject
            {
                ["messaging_product"] = "whatsapp",
                ["to"] = senderPhoneNumber,
                ["text"] = new JObject
                {
                    ["body"] = responseText
                }
            };

            await _sender.SendMessageAsync(responseObj, senderId);
        }
    }
}