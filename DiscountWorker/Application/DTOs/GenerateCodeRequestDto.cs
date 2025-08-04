using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountWorker.Application.DTOs
{
    public class GenerateCodeRequestDto : IncomingMessageDto
    {
        public GenerateCodeParam Params { get; set; }
        
    }

    public class GenerateCodeParam
    {
        public ushort Count { get; set; }
        public byte Length { get; set; }
    }
}