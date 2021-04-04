using Newtonsoft.Json.Linq;

namespace RadioEnlace.Contract.Technical
{
    public interface IDeserialize
    {
        OperationResult<T> Execute<T>(string value);
        OperationResult<T> GetValue<T>(JObject json, string columnName);
    }
}
