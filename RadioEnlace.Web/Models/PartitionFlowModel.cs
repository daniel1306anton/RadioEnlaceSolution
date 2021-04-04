using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadioEnlace.Web.Models
{
    public class PartitionFlowModel
    {
        public ushort Index { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Distance { get; set; }
    }
}