using DiscountWorker.Application.DTOs;
using DiscountWorker.Application.Interfaces;
using DiscountWorker.Domain.Enums;
using System.Text.Json;

namespace DiscountWorker.Application.Services
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IDiscountCodeService _discountCodeService;
        private readonly ILogger<MessageProcessor> _logger;

        public MessageProcessor(
            IDiscountCodeService discountCodeService,
            ILogger<MessageProcessor> logger)
        {
            _discountCodeService = discountCodeService;
            _logger = logger;
        }

        public async Task<ResponseDto> ProcessMessage(string message)
        {
            var response = new ResponseDto { Result = false };

            try
            {
                var request = JsonSerializer.Deserialize<IncomingMessageDto>(message);

                if (request == null)
                {
                    _logger.LogWarning("Received null or invalid message: {Message}", message);
                    return response;
                }

                response.Result = request.Type switch
                {
                    MessageType.Generate => await HandleGenerate(message),
                    MessageType.UseCode => await HandleUseCode(message),
                    _ => LogUnknownType(request.Type)
                };

                _logger.LogInformation("Processed message of type {Type}: {Message}", request.Type, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message: {Message}", message);
            }

            return response;
        }

        private async Task<bool> HandleGenerate(string message)
        {
            var generateRequest = JsonSerializer.Deserialize<GenerateCodeRequestDto>(message);
            if (generateRequest?.Params == null)
            {
                _logger.LogWarning("Invalid GenerateCodeRequestDto: {Message}", message);
                return false;
            }
            _logger.LogInformation("Processing Generate Discount Code");
            return await _discountCodeService.GenerateCodes(generateRequest.Params.Count, generateRequest.Params.Length);
        }

        private async Task<bool> HandleUseCode(string message)
        {
            var useCodeRequest = JsonSerializer.Deserialize<UseCodeRequestDto>(message);
            if (useCodeRequest?.Params == null)
            {
                _logger.LogWarning("Invalid UseCodeRequestDto: {Message}", message);
                return false;
            }
            _logger.LogInformation("Processing Use Discount Code");
            return await _discountCodeService.UseCode(useCodeRequest.Params.Code);
        }

        private bool LogUnknownType(MessageType? type)
        {
            _logger.LogWarning("Unknown message type: {Type}", type);
            return false;
        }
    }
}
