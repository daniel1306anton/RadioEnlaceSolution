using RadioEnlace.Shared.Common;
using RadioEnlace.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Shared.Response
{
    public class EarthProfileResponseDto
    {
        public EarthProfileResponseDto() { Success = true; }
        public EarthProfileResponseDto(List<ErrorDto> errorList)
        {
            Success = false;
            ErrorList = errorList ?? new List<ErrorDto>() { ErrorDto.BuildTechnical("CONTACTE A SU ADMINISTRADOR.")};
        }
        public bool Success { get; set; }
        public List<ErrorDto> ErrorList { get; set; }
        public double Distance { get; set; }
        public List<EarthProfileDto> EarthProfileList { get; set; }
        public bool IsGoodLink { get; set; }
    }
}
