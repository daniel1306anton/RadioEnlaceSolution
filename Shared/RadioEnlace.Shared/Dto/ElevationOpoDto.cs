using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Shared.Dto
{
    public class ElevationOpoDto
    {        
        [JsonProperty("dataset")]
        public string DataSet { get; set; }
        [JsonProperty("elevation")]
        public double Elevation { get; set; }
        [JsonProperty("location")]
        public CordinateOpoDto Location { get; set; }

    }
}
