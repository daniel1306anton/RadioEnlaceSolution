using Newtonsoft.Json;

namespace RadioEnlace.Shared.Dto
{
    public class CoordinateDto
    {
        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }
        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }
        [JsonIgnore]
        public decimal DistanceFlow { get; set; }
        [JsonIgnore]
        public uint IndexFlow { get; set; }
    }
}
