using NLayerApp.CognitiveAIApis.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public class RestOperation : IRestOperation
    {
        Dictionary<string, object> _innerDict = new Dictionary<string, object>();
        public RestOperation(Dictionary<string, object> requestDict = null)
        {
            _innerDict = requestDict ?? new Dictionary<string, object>();
        }
        public IRestEndpoint HasEndpointUri(string endpointUri)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(endpointUri), endpointUri);
            return new RestEndpoint(_innerDict);
        }
    }
}
