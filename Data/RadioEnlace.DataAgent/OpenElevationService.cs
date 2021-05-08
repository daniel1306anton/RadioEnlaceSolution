using RadioEnlace.Contract.Repository;
using RadioEnlace.Contract.Technical;
using RadioEnlace.Shared.Request;
using RadioEnlace.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.DataAgent
{
    public class OpenElevationService : IOpenElevationService
    {
        private readonly Service service;
        public OpenElevationService(Service service)
        {
            this.service = service;
        }
        public OperationResult<OpenElevationResponseDto> GetElevationList(OpenElevationRequestDto request) {

            var countlist = request.CoordinateList.Count();
            var response = new OperationResult<OpenElevationResponseDto>();
            var responseServiceOk = new OpenElevationResponseDto() { ResultList = new List<Shared.Dto.ElevationDto>()};
            var count = 0;
            var ok = true;
            while ((count <= countlist) && countlist >= 1 && ok)
            {
                
                var sendList = request.CoordinateList.Where((x, y) => y >= count &&  y < count + 51).ToList();

                var newReq = new OpenElevationRequestDto() { CoordinateList = sendList };
                var responseService = service.CallServicePost<OpenElevationRequestDto, OpenElevationResponseDto>(newReq, string.Empty);//"api/v1/lookup");
                if (responseService.Failure)
                {
                    response = responseService;
                    ok = false;
                }
                responseServiceOk.ResultList.AddRange(responseService.Result.ResultList);
                count = count + 51;
            }
            return response;
            //return service.CallServicePost<OpenElevationRequestDto, OpenElevationResponseDto>(request, string.Empty);//"api/v1/lookup");

        }
            
    }
}
