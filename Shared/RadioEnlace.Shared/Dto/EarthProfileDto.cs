using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Shared.Dto
{
    public class EarthProfileDto
    {
        public uint IndexFlow { get; set; }
        public double DistanceInit { get; set; }
        public double DistanceInitKm => DistanceInit / 1000;
        public double DistanceEnd { get; set; }
        public double DistanceEndKm => DistanceEnd / 1000;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public double Lm { get; set; }
        public double La { get; set; }
        public double H { get; set; }
        public double Ht { get; set; }
        public double Rf { get; set; }
        public double Zf { get; set; }
        public double E { get; set; }
        public double EZf { get; set; }
    }
}
