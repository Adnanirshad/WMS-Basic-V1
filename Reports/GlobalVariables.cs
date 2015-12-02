using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.Reports
{
    public static class GlobalVariables
    {
        private static bool _DeploymentType;

        public static bool DeploymentType
        {
            get
            {
                return _DeploymentType;
            }
            set
            {
                _DeploymentType = value;
            }
        }
    }
}