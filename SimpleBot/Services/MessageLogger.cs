using AskIT.Models;
using System.Linq;

namespace AskIT.Services
{
    public class MessageLogger
    {
        private readonly WhatsAppDbContext _context;

        public MessageLogger(WhatsAppDbContext context)
        {
            _context = context;
        }

        public void LogMessage(
            string senderPhoneNumber,
            string messageText,
            string senderType,
            int? kbId,
            bool? isMatched,
            double? matchPercent,
            bool needsHumanSupport)
        {
            _context.SaveMessage(senderPhoneNumber, messageText, senderType, kbId, isMatched, matchPercent, needsHumanSupport);
        }

        public Message? GetLastUserMessage(string senderPhoneNumber)
        {
            return _context.Messages
                .Where(m => m.UserPhone == senderPhoneNumber && m.MessageText != "0")
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefault();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}