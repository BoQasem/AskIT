using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using AskIT.Services;

namespace AskIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {
        private readonly MessageProcessor _messageProcessor;

        public WhatsAppController(MessageProcessor messageProcessor)
        {
            _messageProcessor = messageProcessor;
        }

        [HttpGet]
        public IActionResult GetVerification()
        {
            string mode = Request.Query["hub.mode"];
            string challenge = Request.Query["hub.challenge"];
            string verifyToken = Request.Query["hub.verify_token"];
            if (mode == "subscribe" && (verifyToken == "bo qasem" || verifyToken == "bo%20qasem"))
            {
                return Ok(challenge);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveMessage()
        {
            string requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            var receivedMessage = JObject.Parse(requestBody);

            var messages = receivedMessage["entry"]?[0]?["changes"]?[0]?["value"]?["messages"];
            if (messages == null || !messages.Any())
                return Ok();

            string senderPhoneNumber = messages[0]?["from"]?.ToString();
            string incomingMessageText = messages[0]?["text"]?["body"]?.ToString();
            string senderId = receivedMessage["entry"]?[0]?["changes"]?[0]?["value"]?["metadata"]?["phone_number_id"]?.ToString();

            if (string.IsNullOrEmpty(incomingMessageText))
                return Ok();

            await _messageProcessor.ProcessIncomingMessageAsync(senderPhoneNumber, incomingMessageText, senderId);
            return Ok();
        }
    }
}