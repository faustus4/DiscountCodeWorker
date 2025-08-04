using DiscountWorker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountWorker.Application.DTOs
{
    public class IncomingMessageDto
    {
        public MessageType Type { get; set; }
    }
}
