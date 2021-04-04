using RadioEnlace.Contract.AppValues;
using RadioEnlace.Contract.Technical;
using RadioEnlace.Shared.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RadioEnlace.DataAgent
{
    public class Service
    {
        private HttpClient client;
        private readonly IServiceSecurity serviceSecurity;
        private readonly IDeserialize deserialize;
        private readonly ISerialize serialize;

        public Service(IServiceSecurity serviceSecurity, IDeserialize deserialize, ISerialize serialize)
        {
            this.serviceSecurity = serviceSecurity;
            this.deserialize = deserialize;
            this.serialize = serialize;
        }

        private void InitializeClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(serviceSecurity.Url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                 "Basic",
                Convert.ToBase64String(
                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", serviceSecurity.User, serviceSecurity.Password))));
        }

        internal OperationResult<T> CallServicePost<R,T>(R request, string method)
        {
            InitializeClient();
            var jsonRequestProcess = serialize.Execute(request);
            if (jsonRequestProcess.Failure)
            {
                return new OperationResult<T>(jsonRequestProcess.ErrorList);
            }
            HttpContent content = new StringContent(jsonRequestProcess.Result, Encoding.UTF8, "application/json");

            string responseString;
            var url = Path.Combine(serviceSecurity.Url, method);
            try
            {
                var response = client.PostAsync(url, content).Result;
                responseString = response.Content.ReadAsStringAsync().Result;
                
            }
            catch (Exception ex)
            {                
                return new OperationResult<T>(ErrorDto.BuildTechnical(ex.Message));
            }

            return deserialize.Execute<T>(responseString);
        }

        internal OperationResult<T> CallServicePut<T>(string jsonRequest, string method)
        {
            InitializeClient();
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            string responseString;
            try
            {
                var response = client.PutAsync(method, content).Result;
                responseString = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return new OperationResult<T>(ErrorDto.BuildTechnical(ex.Message));
            }

            return deserialize.Execute<T>(responseString);
        }

        internal OperationResult<T> CallServiceGet<T>(string urlGet)
        {
            InitializeClient();

            string responseString;
            try
            {
                var response = client.GetAsync(urlGet).Result;
                responseString = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {                
                return new OperationResult<T>(ErrorDto.BuildTechnical(ex.Message));
            }

            return deserialize.Execute<T>(responseString);
        }
        
    }
}
