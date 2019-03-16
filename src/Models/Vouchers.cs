using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dontKillBill
{
    public class Vouchers
    {
        [Key]
        public int VoucherId { get; set; }
    }
}
