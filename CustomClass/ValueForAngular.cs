using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.CustomClass
{
    public class ValueForAngular
    {
        string CriteriaValue { get; set; }
        string CriteriaName { get; set; }
        public ValueForAngular(string CriteriaValue, string CriteriaName)
        {
            this.CriteriaName = CriteriaName;
            this.CriteriaValue = CriteriaValue;
        
        }
    }
}