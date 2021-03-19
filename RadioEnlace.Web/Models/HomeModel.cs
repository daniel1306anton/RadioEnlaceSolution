﻿using RadioEnlace.Web.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RadioEnlace.Web.Models
{
    public class HomeModel
    {
        public HomeModel()
        {
            Latitude = "19.4980104";
            Longitude = "-99.1367475";
        }
        [Display(ResourceType = typeof(PageResource), Name = "Distance")]
        public string DistanceFlow { get; set; }
        [Display(ResourceType = typeof(PageResource), Name = "Heading")]
        public string HeadingFlow { get; set; }
        [Display(ResourceType = typeof(PageResource), Name = "Latitude")]
        public string Latitude { get; set; }
        [Display(ResourceType = typeof(PageResource), Name = "Longitude")]
        public string Longitude { get; set; }
        [Display(ResourceType = typeof(PageResource), Name = "InitLatitude")]
        public string InitLatitude { get; set; }
        [Display(ResourceType = typeof(PageResource), Name = "InitLongitude")]
        public string InitLongitude { get; set; }
        [Display(ResourceType = typeof(PageResource), Name = "EndLatitude")]
        public string EndLatitude { get; set; }
        [Display(ResourceType = typeof(PageResource), Name = "EndtLongitude")]
        public string EndtLongitude { get; set; }
        [Display(ResourceType = typeof(PageResource), Name = "PartitionFlow")]
        public ushort PartitionFlow { get; set; }
    }
}