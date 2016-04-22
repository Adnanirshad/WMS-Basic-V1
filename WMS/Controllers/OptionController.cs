using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.CustomClass;
using WMS.Models;

namespace WMS.Controllers
{
    public class OptionController : Controller
    {
        //
        // GET: /Option/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UploadImage(HttpPostedFileBase uploadFile)
        {
            if (uploadFile.ContentLength > 0)
            {
                string filePath = Path.GetFileName(uploadFile.FileName);
                uploadFile.SaveAs(GlobalVaribales.ServerPath + filePath);
                //SaveImage("E:\\air.png");
                SaveImage(GlobalVaribales.ServerPath + filePath);
            }
            return RedirectToAction("Index");
        }
        private void SaveImage(string fileaddress)
        {
            //image to byteArray
            Image img = Image.FromFile(fileaddress);
            byte[] bArr = imgToByteArray(img);
            //byte[] bArr = imgToByteConverter(img);
            //Again convert byteArray to image and displayed in a picturebox
            TAS2013Entities ctx = new TAS2013Entities();
            Option oo = new Option();
            oo = ctx.Options.First(aa => aa.ID == 1);
            oo.CompanyLogo = bArr;
            ctx.SaveChanges();
        }
        //convert image to bytearray
        public byte[] imgToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }
	}
}