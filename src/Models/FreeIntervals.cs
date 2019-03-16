using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dontKillBill
{
    public class FreeIntervals
    {

        [Key]
        public int Id { get; set; }
       
        public int StartOfInterval { get; set; }

        public int EndOfInterval { get; set; } 

        public bool Used { get; set; }



    }
}
