using Newtonsoft.Json;
using RadioEnlace.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Shared.Request
{
    public class OpenElevationRequestDto
    {
        [JsonProperty("locations")]
        public List<CoordinateDto> CoordinateList { get; set; }
    }
}
