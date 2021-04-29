using Newtonsoft.Json;
using RadioEnlace.Factory;
using RadioEnlace.Shared.Request;
using RadioEnlace.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadioEnlace.Web.Controllers
{
    public class ProfileEarthController : Controller
    {
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        [HttpPost]
        public JsonResult Execute(HomeModel model)
        {
            var request = new EarthProfileRequestDto() { 
                Distance = Convert.ToDouble(model.DistanceFlow),
                InitLatitude = Convert.ToDecimal(model.InitLatitude),
                InitLongitude = Convert.ToDecimal(model.InitLongitude),
                EndLatitude = Convert.ToDecimal(model.EndLatitude),
                EndLongitude = Convert.ToDecimal(model.EndtLongitude),
                PartitionSeparate = model.SeparateDistance,
                PartitionList = model.PartitionList.Select(x => new Shared.Dto.CoordinateDto() { 
                    DistanceFlow = x.Distance,
                    IndexFlow = x.Index,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                
                }).ToList()


            };
            var response = ProfileEarthFactory.HandleEarthProfileProcess().Execute(request);
            if (!response.Success)
            {
                return Json(new
                {
                    Success = false,
                    ErrorMessage = response.ErrorList.Select(x=> x.Message).Aggregate((x,y) => x+"," +y)

                });
            }
            else
            {
                var lmList = Content(JsonConvert.SerializeObject(response.EarthProfileList.Select(dp => new DataPoint(dp.DistanceInit, dp.Lm)).ToList(), _jsonSetting), "application/json"); 
                var laList = Content(JsonConvert.SerializeObject(response.EarthProfileList.Select(dp => new DataPoint(dp.DistanceInit, dp.La)).ToList(), _jsonSetting), "application/json");
                var htList = Content(JsonConvert.SerializeObject(response.EarthProfileList.Select(dp => new DataPoint(dp.DistanceInit, dp.Ht)).ToList(), _jsonSetting), "application/json");
                var zfList = Content(JsonConvert.SerializeObject(response.EarthProfileList.Select(dp => new DataPoint(dp.DistanceInit, dp.Zf)).ToList(), _jsonSetting), "application/json");
                var tbTable = ConvertViewToString("~/Views/Home/_ProfileDataTable.cshtml", response);
                return Json(new
                {
                    Success = true,
                    LmList = lmList,
                    LaList = laList,
                    HtList = htList,
                    ZfList = zfList,
                    TableData = tbTable
                });
            }
             
        }

        private string ConvertViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext,viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext,writer);
                return writer.ToString();
            }
        }
    }
}