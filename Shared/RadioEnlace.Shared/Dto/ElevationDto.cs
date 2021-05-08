using Newtonsoft.Json;

namespace RadioEnlace.Shared.Dto
{
    public class ElevationDto : CoordinateDto
    {
        [JsonProperty("elevation")]
        public double Elevation { get; set; }
        [JsonProperty("error")]
        public string Error{ get; set; }
    }
}
