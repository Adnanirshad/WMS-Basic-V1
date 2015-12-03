using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WMS.CustomClass
{
    public class CustomFunc
    {
        public bool IsAllLetters(string s)
        {
            //bool result = name.All(x => char.IsLetter(x) || x == ' ' || x == '.' || x == ',');
            bool result = s.All(x => char.IsLetter(x) || x == ' ');

            //foreach (char c in s)
            //{
            //    if (!Char.IsLetter(c))
            //        return false;
            //}
            return result;
        }


        
       
    }
}