using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.InteropServices;

namespace ImageFileSize.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);

                    //// method 1: takes too long
                    //Bitmap img = (Bitmap)Image.FromStream(file.InputStream, true, true);
                    //Bitmap newImg = new Bitmap(img.Width, img.Height);
                    //for (int i = 0; i < img.Width; i++)
                    //{
                    //    for (int j = 0; j < img.Height; j++)
                    //    {
                    //        Color pixel = img.GetPixel(i, j);
                    //        newImg.SetPixel(i, j, pixel);
                    //    }
                    //}
                    //newImg.Save(_path, ImageFormat.Jpeg);

                    //// method 2: size reduction minimal
                    //Bitmap img = (Bitmap)Image.FromStream(file.InputStream, true, true).Clone();
                    //// doesn't do anything to help with filesize
                    //foreach (int id in img.PropertyIdList)
                    //{
                    //    Debug.WriteLine(id);
                    //    img.RemovePropertyItem(id);
                    //}
                    //img.Save(_path, ImageFormat.Jpeg);

                    //// method 3: no color and image is distorted
                    //Bitmap srcBitmap = (Bitmap)Image.FromStream(file.InputStream, true, true);
                    //Bitmap result = new Bitmap(srcBitmap.Width, srcBitmap.Height, PixelFormat.Format32bppArgb);

                    //Rectangle bmpBounds = new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height);
                    //BitmapData srcData = srcBitmap.LockBits(bmpBounds, ImageLockMode.ReadOnly, srcBitmap.PixelFormat);
                    //BitmapData resData = result.LockBits(bmpBounds, ImageLockMode.WriteOnly, result.PixelFormat);

                    //Int64 srcScan0 = srcData.Scan0.ToInt64();
                    //Int64 resScan0 = resData.Scan0.ToInt64();
                    //int srcStride = srcData.Stride;
                    //int resStride = resData.Stride;
                    //int rowLength = Math.Abs(srcData.Stride);
                    //try
                    //{
                    //    byte[] buffer = new byte[rowLength];
                    //    for (int y = 0; y < srcData.Height; y++)
                    //    {
                    //        Marshal.Copy(new IntPtr(srcScan0 + y * srcStride), buffer, 0, rowLength);
                    //        Marshal.Copy(buffer, 0, new IntPtr(resScan0 + y * resStride), rowLength);
                    //    }
                    //}
                    //finally
                    //{
                    //    srcBitmap.UnlockBits(srcData);
                    //    result.UnlockBits(resData);
                    //}
                    //result.Save(_path, ImageFormat.Jpeg);

                    // method 4: WORKS!!!
                    Bitmap src = (Bitmap)Image.FromStream(file.InputStream, true, true);
                    Bitmap target = new Bitmap(src.Width, src.Height);
                    Debug.WriteLine(src.Width + ", " + src.Height);
                    Graphics g = Graphics.FromImage(target);
                    g.DrawRectangle(new Pen(new SolidBrush(Color.White)), 0, 0, target.Width, target.Height);
                    g.DrawImage(src, 0, 0, target.Width, target.Height);
                    target.Save(_path, ImageFormat.Jpeg);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }
    }
}