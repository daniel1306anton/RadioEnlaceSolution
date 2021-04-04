using RadioEnlace.Business;
using RadioEnlace.Contract.AppValues;
using RadioEnlace.Contract.Repository;
using RadioEnlace.Contract.Technical;
using RadioEnlace.DataAgent;
using RadioEnlace.Framework.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Factory
{
    public static class ProfileEarthFactory
    {
        public static HandleEarthProfileProcess HandleEarthProfileProcess()
        {
            IServiceSecurity serviceSecurity = new ServiceSecurityValue();
            IDeserialize deserialize = new JsonDeserialize();
            ISerialize serialize = new JsonSerialize();
            Service service = new Service(serviceSecurity,deserialize,serialize);
            IOpenElevationService openElevationService = new OpenElevationService(service);
            SetElevationEarth setElevationEarth = new SetElevationEarth(openElevationService);
            return new HandleEarthProfileProcess(setElevationEarth);

        }
    }
}
