using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace NLayerApp.CognitiveAIApis.Infrastructure
{
    public class RestOperationHandler<TResult> : IRequestHandler<Dictionary<string, object>, ResponseWrapper<TResult>> where TResult: class
    {
        readonly Func<Dictionary<string, object>, HttpRequestMessage> PostRequestAction;
        readonly Func<HttpResponseMessage, ResponseWrapper<TResult>> PreRequestAction;
        readonly Dictionary<string, object> OperationRequest;
        public RestOperationHandler(
            Dictionary<string, object> operationRequest,
            Func<Dictionary<string, object>, HttpRequestMessage> postRequestAction = null,
            Func<HttpResponseMessage, ResponseWrapper<TResult>> preRequestAction = null)
        {
            var headers = operationRequest.ContainsKey("Headers") 
                ? (Dictionary<string, string>)operationRequest["Headers"] 
                : null;

            OperationRequest = operationRequest;

            PostRequestAction = 
                postRequestAction == null 
                ?  (!headers.ContainsKey("Content-Type") || headers["Content-Type"] == "application/json" 
                        ? GetPostRequestAction() 
                        : GetPostStreamRequestAction())
                : postRequestAction;

            PreRequestAction = preRequestAction == null 
                ? GetPreRequestAction() : preRequestAction;
        }

        public async Task<ResponseWrapper<TResult>> HandleRequestAsync(Dictionary<string, object> apiRequest = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            apiRequest = apiRequest ?? OperationRequest;

            var validationResults = new List<ValidationResult>();

            if (!ValidateApiCallRequest(apiRequest["RequestObject"], out validationResults))
                return new ResponseWrapper<TResult>()
                {
                    IsSuccessfull = false,
                    ReasonPhrase = "Validation Error",
                    ValidationResults = validationResults
                };

            using (var client = new HttpClient())
            {
                var request = PostRequestAction?.Invoke(apiRequest);

                AddRequestUri(apiRequest, ref request);
                AddHeaders((Dictionary<string, string>)apiRequest["Headers"], ref request);

                return await Retry
                    .WithIncremental(retryCount: 5, initialInterval: TimeSpan.FromSeconds(1),
                        increment: TimeSpan.FromSeconds(1))
                    .Catch<OperationCanceledException>()
                    .Catch<HttpRequestException>(exception => exception is HttpRequestException && response.StatusCode == HttpStatusCode.RequestTimeout)
                    .ExecuteAction(async () =>
                    {
                        response = await client.SendAsync(request);
                        var resultToReturn = PreRequestAction?.Invoke(response);
                        return resultToReturn;
                    });
            } 
        }

        private bool ValidateApiCallRequest(object requestObject, 
            out List<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(requestObject);

            Validator.TryValidateObject(requestObject, context, validationResults);

            return validationResults.Count == 0;
        }

        private void AddRequestUri(Dictionary<string, object> apiRequest, 
            ref HttpRequestMessage request)
        {
            var parameters = apiRequest.ContainsKey("Parameters") ? (Dictionary<string, string>)apiRequest["Parameters"] : default;

            var queryString = parameters?.Count > 0 ? $"?{GetParametersString(parameters)}" : "";

            var subPath = apiRequest.ContainsKey("Operation_SubPath") 
                ? $"/{apiRequest["Operation_SubPath"]}" : "";

            var requestUriString = apiRequest.ContainsKey("Endpoint_Version") 
                ? $"{apiRequest["Endpoint_Uri"]}/{apiRequest["Endpoint_Version"]}/{apiRequest["Operation_Path"]}{subPath}{queryString}"
                : $"{apiRequest["Endpoint_Uri"]}/{apiRequest["Operation_Path"]}{subPath}{queryString}";
            
            request.Method = new HttpMethod((string)apiRequest["Operation_Method"]);
            request.RequestUri = new Uri(requestUriString);
        }

        private string GetParametersString(Dictionary<string, string> parameters)
        {
            var result = parameters?.Aggregate("", (k, n) => { return $"{n.Key}={n.Value}&"; });
            result = result.Remove(result.Length - 1, 1);
            return result;
        }

        private void AddHeaders(Dictionary<string, string> headers, 
            ref HttpRequestMessage request)
        {
            foreach (var item in headers)
            {
                request.Headers.Add(item.Key, item.Value);
            }
        }

        private static Func<Dictionary<string, object>, HttpRequestMessage> GetPostRequestAction()
        {
            return new Func<Dictionary<string, object>, HttpRequestMessage>((request) =>
            {
                var json = JsonConvert.SerializeObject(request["RequestObject"]);
                var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var requestMessage = new HttpRequestMessage() { Content = byteArrayContent };

                return requestMessage;
            });
        }
        private static Func<Dictionary<string, object>, HttpRequestMessage> GetPostStreamRequestAction()
        {
            return (request) =>
            {
                var bytes = request["RequestObject"] as byte[];
                var byteArrayContent = new ByteArrayContent(bytes);
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var requestMessage = new HttpRequestMessage() { Content = byteArrayContent };

                return requestMessage;
            };
        }

        private static Func<HttpResponseMessage, ResponseWrapper<TResult>> GetPreRequestAction()
        {
            return new Func<HttpResponseMessage, ResponseWrapper<TResult>>((responseMessage) =>
            {
                var resultToReturn = new ResponseWrapper<TResult>()
                {
                    ResponseContent = null,
                    IsSuccessfull = responseMessage.IsSuccessStatusCode,
                    ReasonPhrase = responseMessage.ReasonPhrase,
                    StatusCode = Enum.GetName(typeof(HttpStatusCode),
                        responseMessage.StatusCode)
                };

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonContent = responseMessage.Content.ReadAsStringAsync().Result;
                    resultToReturn.ResponseContent = typeof(TResult) == typeof(object) 
                        ? (TResult)JsonConvert.DeserializeObject(jsonContent)
                        : JsonConvert.DeserializeObject<TResult>(jsonContent);
                }
                return resultToReturn;
            });
        }
    }
}
