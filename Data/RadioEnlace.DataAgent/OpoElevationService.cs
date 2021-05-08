using RadioEnlace.Contract.Repository;
using RadioEnlace.Contract.Technical;
using RadioEnlace.Shared.Common;
using RadioEnlace.Shared.Request;
using RadioEnlace.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.DataAgent
{
    public class OpoElevationService : IOpenElevationService
    {
        private readonly Service service;
        private readonly string url;
        public OpoElevationService(Service service, string url)
        {
            this.service = service;
            this.url = url;
        }
        public OperationResult<OpenElevationResponseDto> GetElevationList(OpenElevationRequestDto request)
        {

            var countlist = request.CoordinateList.Count();
            var response = new OperationResult<OpenElevationResponseDto>();
            var responseServiceOk = new OpenElevationResponseDto() { ResultList = new List<Shared.Dto.ElevationDto>() };
            var count = 0;
            var ok = true;
            while ((count <= countlist) && countlist >= 1 && ok)
            {

                var sendList = request.CoordinateList.Where((x, y) => y >= count && y < count + 98).ToList();

                if (sendList.Any())
                {
                    var newUrl = url + "/nzdem8m,mapzen?locations=";
                    var locations = sendList.Select(x => x.Latitude.ToString() + "," + x.Longitude.ToString())
                        .Aggregate((x, y) => x + "|" + y);

                    newUrl = newUrl + locations;

                    var responseService = service.CallServiceGet<OpoElevationResponseDto>(newUrl);
                    if (responseService.Failure)
                    {
                        response = new OperationResult<OpenElevationResponseDto>(responseService.ErrorList);
                        ok = false;
                    }

                    if (responseService.Result.Status != "OK")
                    {
                        response = new OperationResult<OpenElevationResponseDto>(ErrorDto.BuildTechnical(responseService.Result.Status));
                        ok = false;
                    }
                    var resulConvertList = responseService.Result.ResultList.Select(x => new Shared.Dto.ElevationDto()
                    {
                        Latitude = x.Location.Latitude,
                        Longitude = x.Location.Longitude,
                        Elevation = x.Elevation
                    });
                    responseServiceOk.ResultList.AddRange(resulConvertList);
                }
                count = count + 98;
            }
            return new OperationResult<OpenElevationResponseDto>(responseServiceOk);            

        }
    }
}
