using RadioEnlace.Shared.Request;
using RadioEnlace.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Business
{
    public class HandleEarthProfileProcess
    {
        private readonly SetElevationEarth setElevationEarth;
        public HandleEarthProfileProcess(SetElevationEarth setElevationEarth)
        {
            this.setElevationEarth = setElevationEarth;
        }
        public EarthProfileResponseDto Execute(EarthProfileRequestDto request)
        {
            var response = new EarthProfileResponseDto();
            var processElevation = setElevationEarth.Execute(request);
            if (processElevation.Failure)
            {
                return new EarthProfileResponseDto(processElevation.ErrorList);
            }
            var earthProfileList = processElevation.Result;

            earthProfileList = new CalculateProfileEarth().Execute(earthProfileList,request);
            response.Distance = request.Distance;
            response.EarthProfileList = earthProfileList;
            return response;
        }
    }
}
