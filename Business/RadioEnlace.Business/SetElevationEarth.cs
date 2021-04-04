using RadioEnlace.Contract.Repository;
using RadioEnlace.Contract.Technical;
using RadioEnlace.Shared.Dto;
using RadioEnlace.Shared.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Business
{
    public class SetElevationEarth
    {
        private readonly IOpenElevationService openElevationService;
        public SetElevationEarth(IOpenElevationService openElevationService)
        {
            this.openElevationService = openElevationService;
        }
        public OperationResult<List<EarthProfileDto>> Execute(EarthProfileRequestDto request)
        {
            return new OperationResult<List<EarthProfileDto>>();
        }
    }
}
