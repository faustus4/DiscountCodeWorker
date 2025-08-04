using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountWorker.Domain.Entities
{
    [Index(nameof(Code), IsUnique = true)]
    public class DiscountCode
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(8)")]
        public required string Code { get; set; }
        public bool IsUsed { get; set; }

    }
}
