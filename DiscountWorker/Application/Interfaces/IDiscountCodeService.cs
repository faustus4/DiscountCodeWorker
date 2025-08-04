using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountWorker.Application.Interfaces
{
    public interface IDiscountCodeService
    {
        Task<bool> GenerateCodes(ushort? count, byte? length);
        Task<bool> UseCode(string? code);
    }
}
