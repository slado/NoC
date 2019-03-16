using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dontKillBill
{ 
    public static class FindFreeIntervals
    {
                 
        public static  void Find(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            var vouchers =  context.Vouchers.OrderBy(v => v.VoucherId).ToList();

            List<FreeIntervals> freeIntervals = new List<FreeIntervals>();

            int start = 1;

            foreach (var record in vouchers)
            {              

                if (start <= record.VoucherId)
                {
                    var interval = new FreeIntervals { StartOfInterval = start, EndOfInterval = record.VoucherId, Used = false };

                    freeIntervals.Add(interval);
                    
                }
                start = record.VoucherId+1;
            }


            foreach(FreeIntervals interval in freeIntervals)
            {
                context.Add(interval);
            }

            context.SaveChanges();
        }
    }
}
