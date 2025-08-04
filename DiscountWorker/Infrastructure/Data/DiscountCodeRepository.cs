using DiscountWorker.Domain.Entities;
using DiscountWorker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountWorker.Infrastructure.Data
{
    public class DiscountCodeRepository : IDiscountCodeRepository
    {
        private readonly AppDbContext _dbContext;

        public DiscountCodeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(DiscountCode code)
        {
            _dbContext.DiscountCodes.Add(code);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<DiscountCode>> GetAllAsync()
        {
            return await _dbContext.DiscountCodes.ToListAsync();
        }

        public async Task<List<string>> GetAllCodes()
        {
            return await _dbContext.DiscountCodes.Select(c => c.Code).ToListAsync();
        }

        public async Task<int> InsertCodes(HashSet<string> codes)
        {
            var discountCodes = codes.Select(code => new DiscountCode { Code = code }).ToList();
            _dbContext.DiscountCodes.AddRange(discountCodes);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UseCode(string code)
        {
            var discountCode = _dbContext.DiscountCodes.FirstOrDefault(c => c.Code == code);
            if (discountCode == null)
            {
                return 0; // Code not found
            }

            discountCode.IsUsed = true;

            return await _dbContext.SaveChangesAsync();
        }
    }
}
