using DiscountWorker.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountWorker.Application.Interfaces
{
    public interface IMessageProcessor
    {
        Task<ResponseDto> ProcessMessage(string message);
    }
}
