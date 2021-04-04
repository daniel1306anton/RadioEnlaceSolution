using RadioEnlace.Factory;
using RadioEnlace.Shared.Request;
using RadioEnlace.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadioEnlace.Web.Controllers
{
    public class ProfileEarthController : Controller
    {        
        [HttpPost]
        public JsonResult Execute(HomeModel model)
        {
            var request = new EarthProfileRequestDto() { 
                Distance = Convert.ToDecimal(model.DistanceFlow),
                InitLatitude = Convert.ToDecimal(model.InitLatitude),
                InitLongitude = Convert.ToDecimal(model.InitLongitude),
                EndLatitude = Convert.ToDecimal(model.EndLatitude),
                EndLongitude = Convert.ToDecimal(model.EndtLongitude),
                PartitionList = model.PartitionList.Select(x => new Shared.Dto.CoordinateDto() { 
                    DistanceFlow = x.Distance,
                    IndexFlow = x.Index,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                
                }).ToList()


            };
            var response = ProfileEarthFactory.HandleEarthProfileProcess().Execute(request);
             return Json(new
            {
                
            });
        }
    }
}