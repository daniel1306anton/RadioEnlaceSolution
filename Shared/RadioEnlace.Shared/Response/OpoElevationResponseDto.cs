using Newtonsoft.Json;
using RadioEnlace.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Shared.Response
{
    public class OpoElevationResponseDto
    {
        [JsonProperty("results")]
        public List<ElevationOpoDto> ResultList { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
