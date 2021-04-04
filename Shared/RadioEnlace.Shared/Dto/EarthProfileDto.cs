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
        public decimal DistanceInit { get; set; }
        public decimal DistanceEnd { get; set; }
        public uint Lm { get; set; }
        public decimal La { get; set; }
        public decimal H { get; set; }
        public decimal Ht { get; set; }
        public decimal Rf { get; set; }
        public decimal Zf { get; set; }
        public decimal E { get; set; }
        public decimal EZf { get; set; }
    }
}
