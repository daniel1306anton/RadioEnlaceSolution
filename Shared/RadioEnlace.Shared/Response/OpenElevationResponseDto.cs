using Newtonsoft.Json;
using RadioEnlace.Shared.Dto;
using System.Collections.Generic;

namespace RadioEnlace.Shared.Response
{
    public class OpenElevationResponseDto
    {
        [JsonProperty("results")]
        public List<ElevationDto> ResultList { get; set; }
    }
}
