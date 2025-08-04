using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountWorker.Application.DTOs
{
    public class UseCodeRequestDto : IncomingMessageDto
    {
       public UseCodeRequestParam Params { get; set; }
    }

    public class UseCodeRequestParam
    {
        public string Code { get; set; }
    }
}
