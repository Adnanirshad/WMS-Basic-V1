using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMSLibrary;

namespace WMS.CustomClass
{
    public class SessionManager
    {
        public static FiltersModel filterModel;
        public void InitSession()
        {
            filterModel = new FiltersModel();
        }

        internal static FiltersModel Init()
        {
            if (filterModel == null)
                filterModel = new FiltersModel();
            return filterModel;
        }
    }
}