using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.CustomClass
{
    public static class GlobalVaribales
    {
        public static string ServerPath { get; set; }
        public static string CompanyName { get; set; }
        public static string NoOfEmps { get; set; }
        public static string NoOfUsers { get; set; }
        public static string NoOfDevices { get; set; }
        public static string DeviceType { get; set; }
        public static string LicenseType { get; set; }
         //DeviceType == 1 : RFID
        //DeviceType == 2 : FP
        //DeviceType==3 : Face 
    }
}