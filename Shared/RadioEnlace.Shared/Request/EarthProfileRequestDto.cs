using RadioEnlace.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Shared.Request
{
    public class EarthProfileRequestDto
    {
        public double Distance { get; set; }
        public decimal InitLatitude { get; set; }
        public decimal InitLongitude { get; set; }
        public decimal EndLatitude { get; set; }
        public decimal EndLongitude { get; set; }
        public decimal PartitionSeparate { get; set; }
        public double H1 { get; set; }
        public double H2 { get; set; }
        public List<CoordinateDto> PartitionList { get; set; }

    }
}
