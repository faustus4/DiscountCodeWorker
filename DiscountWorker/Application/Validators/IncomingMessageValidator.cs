using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountWorker.Application.Validators
{
    public class IncomingMessageValidator
    {
        public static void ValidateCountAndLength(ushort? count, byte? length)
        {
            if (count is null)
                throw new ArgumentNullException(nameof(count), "Count must be provided.");
            if (length is null)
                throw new ArgumentNullException(nameof(length), "Length must be provided.");
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            if (count > 2000)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must not exceed 2000.");
            if (length < 7 || length > 8)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be between 7 and 8 characters.");
        }

        public static void ValidateCode(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code), "Code must be provided.");
            if (code.Length < 7 || code.Length > 8)
                throw new ArgumentOutOfRangeException(nameof(code), "Code length must be between 7 and 8 characters.");
        }
    }
}
