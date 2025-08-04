using DiscountWorker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountWorker.Domain.Interfaces
{
    public interface IDiscountCodeRepository
    {
        Task SaveAsync(DiscountCode code);
        Task<List<DiscountCode>> GetAllAsync();

        Task<List<string>> GetAllCodes();
        Task<int> InsertCodes(HashSet<string> codes);
        Task<int> UseCode(string code);
    }
}
