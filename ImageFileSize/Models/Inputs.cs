using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageFileSize.Models
{
    public class Inputs
    {
        public HttpPostedFileBase file { get; set; }
        public int maxWidth { get; set; }
    }
}