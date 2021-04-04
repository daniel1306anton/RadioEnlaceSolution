using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RadioEnlace.Contract.Technical;
using RadioEnlace.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.Framework.Json
{
    public class JsonDeserialize : IDeserialize
    {
        private const string FAILURE_DESERIALIZE_OBJECT = "Ocurrio un error al tratar de deserializar el archivo.";
        public OperationResult<T> Execute<T>(string value)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(value);
                return new OperationResult<T>(obj);
            }
            catch (Exception ex)
            {                
                return new OperationResult<T>(ErrorDto.BuildTechnical(FAILURE_DESERIALIZE_OBJECT));
            }
        }
        public OperationResult<T> GetValue<T>(JObject json, string columnName)
        {
            try
            {
                var valueColumn = json.GetValue(columnName).ToString();
                var obj = JsonConvert.DeserializeObject<T>(valueColumn);
                return new OperationResult<T>(obj);
            }
            catch (Exception ex)
            {                
                return new OperationResult<T>(ErrorDto.BuildTechnical(FAILURE_DESERIALIZE_OBJECT));
            }
        }
    }
}
