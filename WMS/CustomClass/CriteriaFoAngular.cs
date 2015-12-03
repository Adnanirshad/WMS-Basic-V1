using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.CustomClass
{
    public class CriteriaFoAngular
    {
        public CriteriaFoAngular(string id, string name)
        {
            this.name = name;
            this.id = id;
        
        
        }
        public string id { get; set; }
        public string name { get; set; }
    }
}