using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadioEnlace.Web.Models
{
    public class DataPoint
    {
        public DataPoint(double y, string label)
        {
            this.Y = y;
            this.Label = label;
        }

        public DataPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }


        public DataPoint(double x, double y, string label)
        {
            this.X = x;
            this.Y = y;
            this.Label = label;
        }

        public DataPoint(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public DataPoint(double x, double y, double z, string label)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Label = label;
        }


        //Explicitly setting the name to be used while serializing to JSON. 
        [JsonProperty("label")]        
        public string Label = null;

        //Explicitly setting the name to be used while serializing to JSON.        
        [JsonProperty("y")]
        public Nullable<double> Y = null;

        //Explicitly setting the name to be used while serializing to JSON.        
        [JsonProperty("x")]
        public Nullable<double> X = null;

        //Explicitly setting the name to be used while serializing to JSON.        
        [JsonProperty("z")]
        public Nullable<double> Z = null;
    }
}