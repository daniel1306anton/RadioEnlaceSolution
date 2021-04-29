using RadioEnlace.Contract.Repository;
using RadioEnlace.Contract.Technical;
using RadioEnlace.Shared.Dto;
using RadioEnlace.Shared.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Business
{
    public class SetElevationEarth
    {
        private readonly IOpenElevationService openElevationService;
        public SetElevationEarth(IOpenElevationService openElevationService)
        {
            this.openElevationService = openElevationService;
        }
        public OperationResult<List<EarthProfileDto>> Execute(EarthProfileRequestDto request)
        {
            decimal distanceRunInit = 0;
            decimal distanceRunEnd = (decimal)request.Distance;
            var earthProfileList = new List<EarthProfileDto>() { 
                new EarthProfileDto(){
                    DistanceInit = (double)distanceRunInit,
                    DistanceEnd = (double)distanceRunEnd,
                    Latitude = request.InitLatitude,
                    Longitude = request.InitLongitude,
                    IndexFlow = 0                                   
                }             
            };


            request.PartitionList.ForEach(x => {
                distanceRunInit = distanceRunInit + request.PartitionSeparate;
                distanceRunEnd = distanceRunEnd - request.PartitionSeparate;

                earthProfileList.Add(new EarthProfileDto() { 
                    DistanceInit = (double)distanceRunInit,
                    DistanceEnd = (double)distanceRunEnd,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    IndexFlow = x.IndexFlow
                
                });
            
            });
            earthProfileList.Add(new EarthProfileDto() { 
                    DistanceInit = request.Distance,
                    DistanceEnd = 0,
                    Latitude = request.EndLatitude,
                    Longitude = request.EndLongitude,
                    IndexFlow = earthProfileList.Max(x => x.IndexFlow) + 1              
                });

            //seteamos elevacion terreno

            var elevationProcess = openElevationService.GetElevationList(new OpenElevationRequestDto() { 
                CoordinateList = earthProfileList.Select(x => new CoordinateDto() { 
                 IndexFlow = x.IndexFlow,
                 Latitude = x.Latitude,
                 Longitude = x.Longitude
                }).ToList()
            
            });

            if (elevationProcess.Failure)
            {
                return new OperationResult<List<EarthProfileDto>>(elevationProcess.ErrorList);
            }
            var elevationList = elevationProcess.Result.ResultList;

            var join = (from req in earthProfileList
                       join res in elevationList on new { req.Latitude, req.Longitude } equals new { res.Latitude, res.Longitude }
                       select new EarthProfileDto() { 
                           DistanceEnd = req.DistanceEnd,
                           DistanceInit = req.DistanceInit,
                           Latitude = req.Latitude,
                           Longitude = req.Longitude,
                           IndexFlow = req.IndexFlow,
                           Lm = res.Elevation                       
                       }).ToList();

            return new OperationResult<List<EarthProfileDto>>(join);
        }
    }
}
