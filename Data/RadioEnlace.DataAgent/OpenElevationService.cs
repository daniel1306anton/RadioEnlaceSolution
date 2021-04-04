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
        public OperationResult<OpenElevationResponseDto> GetElevationList(OpenElevationRequestDto request) =>
            service.CallServicePost<OpenElevationRequestDto, OpenElevationResponseDto>(request, "api/v1/lookup");
    }
}
