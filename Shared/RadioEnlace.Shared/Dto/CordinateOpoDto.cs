using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Shared.Dto
{
    public class CordinateOpoDto
    {
        [JsonProperty("lat")]
        public decimal Latitude { get; set; }
        [JsonProperty("lng")]
        public decimal Longitude { get; set; }
    }
}
