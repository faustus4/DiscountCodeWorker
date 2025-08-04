using DiscountWorker.Application.Interfaces;
using DiscountWorker.Application.Validators;
using DiscountWorker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountWorker.Application.Services
{
    public class DiscountCodeService : IDiscountCodeService
    {
        private readonly IDiscountCodeRepository _discountCodeRepository;

        public DiscountCodeService(IDiscountCodeRepository discountCodeRepository)
        {
            _discountCodeRepository = discountCodeRepository;
        }

        public async Task<bool> GenerateCodes(ushort? count, byte? length)
        {
            IncomingMessageValidator.ValidateCountAndLength(count, length);

            Random random = new Random();
            var codeLength = length ?? 7;
            var retry = true;
            var retryCount = 0;
            var retryMax = 10;

            while (retry && retryCount <= retryMax)
            {
                HashSet<string> codes = new HashSet<string>();
                while (codes.Count < count)
                {
                    var code = GenerateRandomCode(codeLength, random);
                    codes.Add(code);
                }

                var insertedCount = await _discountCodeRepository.InsertCodes(codes);
                retry = insertedCount != count; // Retry regeneration if code insertion failed
                retryCount++;
            }

            return retryCount<retryMax;
        }

        public async Task<bool> UseCode(string? code)
        {
            IncomingMessageValidator.ValidateCode(code);
            var discountCode = code ?? string.Empty;
            var useCodeCount = await _discountCodeRepository.UseCode(discountCode);
            return useCodeCount == 1;
        }

        private string GenerateRandomCode(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
