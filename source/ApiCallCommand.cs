using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure
{
    public class ApiCallCommand<TRequest> : IRequest<TRequest>
    {
        public ApiCallCommand(TRequest requestObject)
        {
            RequestObject = requestObject;
        }
        public TRequest RequestObject { get; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public string Method { get; set; }
        public string EndpointUri { get; set; }
        public string ApiPath { get; set; }
        public object Version { get; set; }
        public string RequestName { get; set; }
        public string SubPath { get; set; }
        public string SubscriptionKey { get; set; }
        public Func<ApiCallCommand<TRequest>, HttpRequestMessage> PostRequestAction { get; set; }
        public Func<HttpResponseMessage, object> PreRequestAction { get; set; }
    }


    public class ApiCallCommand<TRequest, TResponse> : IRequest<TRequest>
    {
        public ApiCallCommand(TRequest requestObject)
        {
            RequestObject = requestObject;
        }
        public TRequest RequestObject { get; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public string Endpoint_Uri { get; set; }
        public string Endpoint_Version { get; set; }
        public string Operation_Method { get; set; }
        public string Operation_Path { get; set; }
        public string Operation_SubPath { get; set; }
        public string Operation_PreSubPath { get; set; }
        public string Operation_PostSubPath { get; set; }
        public string SubscriptionKey { get; set; }
        public Func<TRequest, HttpRequestMessage> PostRequestAction { get; set; }
        public Func<HttpResponseMessage, TResponse> PreRequestAction { get; set; }
    }

}
