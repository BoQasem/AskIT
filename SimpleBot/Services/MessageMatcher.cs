using AskIT.Models;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace AskIT.Services
{
    public class MessageMatcher 
    {
        private readonly WhatsAppDbContext _context;
        private readonly IConfiguration _configuration;

        public MessageMatcher(WhatsAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public MatchResult? FindBestMatch(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return null;

            var stopwords = new[] { "how", "why","what","about", "to", "a", "in", "the", "for", "on", "at", "i", "my", "is", "do","can" };
            var tokens = userMessage.ToLower().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            var nonStopTokens = tokens.Except(stopwords).ToArray();

            if (nonStopTokens.Length == 0)
                return null;

            var allKb = _context.KnowledgeBases.ToList();
            MatchResult? best = null;
            double bestPercent = 0;
            foreach (var kb in allKb)
            {
                double percent = CalculateSimilarity(nonStopTokens, kb.QuestionText, stopwords);
                if (percent > bestPercent)
                {
                    bestPercent = percent;
                    best = new MatchResult
                    {
                        KnowledgeBaseId = kb.Id,
                        IdealAnswer = kb.AnswerText,
                        MatchPercent = percent
                    };
                }
            }

            double threshold = Convert.ToDouble(_configuration["BotSettings:GlobalPercentage"]);
            return best; 
        }

        private double CalculateSimilarity(string[] userTokens, string kbQuestion, string[] stopwords)
        {
            var kbTokens = kbQuestion.ToLower().Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
                .Except(stopwords)
                .ToArray();

            var userSet = userTokens.ToHashSet();
            var kbSet = kbTokens.ToHashSet();

            int intersection = userSet.Intersect(kbSet).Count();
            int union = userSet.Union(kbSet).Count();

            return (double)intersection / union * 100;
        }
    }

    public class MatchResult
    {
        public int KnowledgeBaseId { get; set; }
        public string IdealAnswer { get; set; }
        public double MatchPercent { get; set; }
    }
}
