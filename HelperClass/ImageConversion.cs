using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WMS.Models;

namespace WMS.HelperClass
{
    public class ImageConversion
    {
        public int UploadImageInDataBase(HttpPostedFileBase file, Emp _Emp)
        {
            using (var context = new TAS2013Entities())
            {
                List<EmpPhoto> _empPhotoList = new List<EmpPhoto>();
                EmpPhoto _EmpPhoto = new EmpPhoto();
                int empPhotoID = 0;
                _empPhotoList = context.EmpPhotoes.Where(aa => aa.EmpID == _Emp.EmpID).ToList();
                _EmpPhoto.EmpPic = ConvertToBytes(file);
                if (_empPhotoList.Count > 0)
                {
                    //Update Existing Image
                    _EmpPhoto.EmpID = _empPhotoList.FirstOrDefault().EmpID;
                    _EmpPhoto.PhotoID = _empPhotoList.FirstOrDefault().PhotoID;
                }
                else
                {
                    //Add New Image
                    _EmpPhoto.EmpID = _Emp.EmpID;
                    context.EmpPhotoes.Add(_EmpPhoto);
                }
                try
                {
                    empPhotoID = _EmpPhoto.PhotoID;
                    context.SaveChanges();
                    return empPhotoID;
                }
                catch (Exception ex)
                {
                    return empPhotoID;
                } 
            }

        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        public int UploadImageInDataBase(HttpPostedFileBase file, string _empNo)
        {
            using (var context = new TAS2013Entities())
            {
                List<EmpPhoto> _empPhotoList = new List<EmpPhoto>();
                EmpPhoto _EmpPhoto = new EmpPhoto();
                Emp _Emp = new Emp();
                int empPhotoID = 0;
                _Emp = context.Emps.First(aa => aa.EmpNo == _empNo);
                _empPhotoList = context.EmpPhotoes.Where(aa => aa.EmpID == _Emp.EmpID).ToList();
                _EmpPhoto.EmpPic = ConvertToBytes(file);
                if (_empPhotoList.Count > 0)
                {
                    //Update Existing Image
                    _EmpPhoto.EmpID = _empPhotoList.FirstOrDefault().EmpID;
                    _EmpPhoto.PhotoID = _empPhotoList.FirstOrDefault().PhotoID;
                }
                else
                {
                    //Add New Image
                    _EmpPhoto.EmpID = _Emp.EmpID;
                    context.EmpPhotoes.Add(_EmpPhoto);
                }
                try
                {
                    empPhotoID = _EmpPhoto.PhotoID;
                    context.SaveChanges();
                    return empPhotoID;
                }
                catch (Exception ex)
                {
                    return empPhotoID;
                }
            }
        }
    }
}