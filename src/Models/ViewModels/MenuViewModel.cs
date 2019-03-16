using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dontKillBill
{
    public class MenuViewModel
    {
        /// <summary>
        /// start of selected interval
        /// </summary>
        [Range(0,int.MaxValue)]
        [Display(Name ="Zaciatok intervalu")]
        public int StartPoint { get; set; }

        /// <summary>
        /// end of selected interval
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Hodnota musi byt kladna")]
        [Display(Name = "Koniec intervalu")]
        public int EndPoint { get; set; }

        /// <summary>
        /// Number of vouchers to be generated
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage ="Hodnota musi byt kladna")]
        [Display(Name = "Pocet voucherov ktore maju byt vygenerovane")]
        public int Count { get; set; }

        /// <summary>
        /// voucher id to be generated to qr code
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Hodnota musi byt kladna")]
        [Display(Name = "Pocet voucherov ktore maju byt vygenerovane")]
        public int ToQr { get; set; }

    }
}
