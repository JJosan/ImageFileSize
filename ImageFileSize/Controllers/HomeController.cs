using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageFileSize.Models;

namespace ImageFileSize.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Inputs input)
        {
            try
            {
                HttpPostedFileBase file = input.file;
                int maxWidth = input.maxWidth;

                if (file == null)
                {
                    ViewBag.Message = "No File Selected";
                    return View();
                }
                if (maxWidth < 0)
                {
                    ViewBag.Message = "Invalid Width";
                    return View();
                }
                if (!file.ContentType.Contains("image"))
                {
                    ViewBag.Message = "Invalid File";
                    return View();
                }


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
                    Graphics g = Graphics.FromImage(target);
                    g.DrawRectangle(new Pen(new SolidBrush(Color.White)), 0, 0, target.Width, target.Height);
                    g.DrawImage(src, 0, 0, target.Width, target.Height);

                    if (maxWidth == 0 || maxWidth >= target.Width)
                    {
                        target.Save(_path, ImageFormat.Jpeg);
                    }
                    else
                    {
                        // resizing
                        double scale = (double)target.Width / maxWidth;
                        int newWidth = (int)Math.Round(target.Width / scale);
                        int newHeight = (int)Math.Round(target.Height / scale);
                        Bitmap resized = new Bitmap(target, new Size(newWidth, newHeight));
                        resized.Save(_path, ImageFormat.Jpeg);
                    }

                }
                ViewBag.Message = "File Uploaded Successfully";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed";
                return View();
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}