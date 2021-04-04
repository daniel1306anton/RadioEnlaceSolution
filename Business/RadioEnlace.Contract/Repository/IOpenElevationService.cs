using RadioEnlace.Contract.Technical;
using RadioEnlace.Shared.Request;
using RadioEnlace.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Contract.Repository
{
    public interface IOpenElevationService
    {
        OperationResult<OpenElevationResponseDto> GetElevationList(OpenElevationRequestDto request);
    }
}
