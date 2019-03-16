using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using QRCoder;

namespace dontKillBill.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
       


        public HomeController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

       
        public IActionResult Index()
        {
            //staci aby to zbehlo raz
           // FindFreeIntervals.Find(_context);

            return View();
          
        }
        public IActionResult SelectInterval(MenuViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Content("Neplatny vstup", "text/html");
            }

            var vouchers = _context.Vouchers.OrderBy(m => m.VoucherId).Skip(model.StartPoint-1).Take(model.EndPoint-model.StartPoint).ToList();

            return View(vouchers);
        }

        public IActionResult NewVoucher(MenuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Content("Neplatny vstup", "text/html");
            }

            GenerateVouchers(viewModel.Count);

            return View();
        }
       

        public IActionResult QrCode(MenuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Content("Neplatny vstup", "text/html");
            }

            string path=string.Empty;
            string exist = Exist(viewModel.ToQr);            

            if(exist == viewModel.ToQr.ToString())
            {
                path = GenerateQrCode(viewModel.ToQr);
            }

            ViewData["exist"] = exist;
            ViewData["qrCode"] = path;
            return View();
        }

        #region Generate new vouchers

        private void GenerateVouchers(int count)
        {
            //ak nieje co vytvarat
            if (count == 0)
                return;

           
            var intervals = _context.FreeIntervals.Where(u => u.Used == false);

            //ak uz nepotrebujem generovat podla intervalov
            if (!intervals.Any())
            {
                GenerateVouchersFromEnd(count);
            }

            //generovanie z intervalov
            else
            {                
                intervals = intervals.OrderBy(s => s.StartOfInterval).Take(count);                
                GenerateVouchersFromInterval(intervals, count);
            }
          
            _context.SaveChanges();
        }

        
        private void GenerateVouchersFromEnd(int count)
        {
            var last = _context.Vouchers.OrderByDescending(v => v.VoucherId).FirstOrDefault();
            int start = last.VoucherId+1;
            int end = last.VoucherId+count;

            for (int i = start; i < end; i++)
            {
                var voucher = new Vouchers { VoucherId = i };
                _context.Add(voucher);
            }
        }

        private void GenerateVouchersFromInterval(IQueryable<FreeIntervals> intervals, int count)
        {
            int generated = 0;

            foreach (var interval in intervals)
            {


                int start = interval.StartOfInterval;
                int end = interval.EndOfInterval;

                for (int i = start; i < end; i++)
                {
                    generated++;
                    var voucher = new Vouchers { VoucherId = i };
                    _context.Add(voucher);

                    if (generated == count)
                    {
                        interval.StartOfInterval = i + 1;
                       
                        return;
                    }
                }
                interval.Used = true;
            }

            //ak sa to dostane az sem treba dogenerovat vouchre aj za vsetkymi intervalmi
            GenerateVouchersFromEnd(count - generated);
        }
        #endregion

        #region QrCode

        private string Exist(int id)
        {
            var result = _context.Vouchers.Find(id);

            return result == null ? "Takyto voucher zatial neexistuje" : id.ToString();
        }

        private string GenerateQrCode(int voucherId)
        {         
            string id = voucherId.ToString();
            var webRoot = _env.WebRootPath;            

            //ak Qr code este nebol vytvoreny, vytvor ho
            if(!FindQrCode(voucherId, webRoot))
            {
                var fullPath = Path.Combine(webRoot, "QrCodes", "voucher_" + id + ".png");

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(id, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                qrCodeImage.Save(fullPath, ImageFormat.Png);     
            }

            return Path.Combine("/QrCodes", "voucher_" + id + ".png");
        }

        private bool FindQrCode(int voucherId,string webRoot)
        {
            var _fileProvider = new PhysicalFileProvider(webRoot);

            var contents = _fileProvider.GetDirectoryContents("/QrCodes");

            var result = contents.ToList().Find(n => n.Name == "voucher_" + voucherId.ToString() + ".png");

            if (result == null)
                return false;

            return true;                                  
        }
        #endregion
    }

}
