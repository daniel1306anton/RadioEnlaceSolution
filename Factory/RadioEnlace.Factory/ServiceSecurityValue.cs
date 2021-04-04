using RadioEnlace.Contract.AppValues;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Factory
{
    public class ServiceSecurityValue : IServiceSecurity
    {
        public string User => GetValue("UserOpenElevation");

        public string Password => GetValue("PassOpenElevation");

        public string Url => GetValue("UrlOpenElevation");

        private string GetValue(string parameter)
        {
            return ConfigurationManager.AppSettings[parameter].ToString();
        }
    }
}
