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
    public class ApiCommandHandler<TRequest, TResult> : IRequestHandler<ApiCallCommand<TRequest, TResult>, TResult> where TResult: class
    {
        readonly Func<TRequest, HttpRequestMessage> PostRequestAction;
        readonly Func<HttpResponseMessage, TResult> PreRequestAction;
        readonly ApiCallCommand<TRequest, TResult> ApiRequest;
        public ApiCommandHandler(
            ApiCallCommand<TRequest, TResult> apiRequest,
            Func<TRequest, HttpRequestMessage> postRequestAction = null,
            Func<HttpResponseMessage, TResult> preRequestAction = null)
        {
            ApiRequest = apiRequest;
            PostRequestAction = postRequestAction == null 
                ? apiRequest.Headers["ContentType"] == "application/json" ? GetPostRequestAction() : GetPostStreamRequestAction()
                : postRequestAction;
            PreRequestAction = preRequestAction == null 
                ? GetPreRequestAction() : preRequestAction;
        }

        public async Task<TResult> HandleRequestAsync(ApiCallCommand<TRequest, TResult> apiRequest = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            apiRequest = apiRequest ?? ApiRequest;
            if (!ValidateApiCallRequest(apiRequest)) return null;
            using (var client = new HttpClient())
            {
                var request = PostRequestAction?.Invoke(apiRequest.RequestObject);

                AddRequestUri(apiRequest, ref request);
                AddHeaders(apiRequest, ref request);

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

        private bool ValidateApiCallRequest(ApiCallCommand<TRequest, TResult> apiRequest)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(apiRequest.RequestObject);

            Validator.TryValidateObject(apiRequest.RequestObject, context, validationResults);

            return validationResults.Count == 0;
        }

        private void AddRequestUri(ApiCallCommand<TRequest, TResult> apiRequest, ref HttpRequestMessage request)
        {
            var queryString = apiRequest.Parameters?.Count > 0 ? $"?{GetParametersString(apiRequest.Parameters)}" : "";
            var subPath = !String.IsNullOrEmpty(apiRequest.Operation_SubPath) ? $"/{apiRequest.Operation_SubPath}" : "";
            var requestUriString = $"{apiRequest.Endpoint_Uri}/{apiRequest.Endpoint_Version}/{apiRequest.Operation_Path}{subPath}{queryString}";
            
            request.Method = new HttpMethod(apiRequest.Operation_Method);
            request.RequestUri = new Uri(requestUriString);
        }

        private string GetParametersString(Dictionary<string, string> parameters)
        {
            var result = parameters?.Aggregate("", (k, n) => { return $"{n.Key}={n.Value}&"; });
            result = result.Remove(result.Length - 1, 1);
            return result;
        }

        private void AddHeaders(ApiCallCommand<TRequest, TResult> apiRequest, ref HttpRequestMessage request)
        {
            foreach (var item in apiRequest.Headers)
            {
                request.Headers.Add(item.Key, item.Value);
            }
        }

        private static Func<TRequest, HttpRequestMessage> GetPostRequestAction()
        {
            return new Func<TRequest, HttpRequestMessage>((request) =>
            {
                var json = JsonConvert.SerializeObject(request);
                var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
                var requestMessage = new HttpRequestMessage() { Content = byteArrayContent };

                return requestMessage;
            });
        }
        private static Func<TRequest, HttpRequestMessage> GetPostStreamRequestAction()
        {
            return (request) =>
            {
                var bytes = request as byte[];
                var byteArrayContent = new ByteArrayContent(bytes);
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var requestMessage = new HttpRequestMessage() { Content = byteArrayContent };

                return requestMessage;
            };
        }

        private static Func<HttpResponseMessage, TResult> GetPreRequestAction()
        {
            return new Func<HttpResponseMessage, TResult>((responseMessage) =>
            {
                var jsonContent = responseMessage.Content.ReadAsStringAsync().Result;
                var resultToReturn = JsonConvert.DeserializeObject<TResult>(jsonContent);
                return resultToReturn;
            });
        }
    }
}
