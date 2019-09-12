using NLayerApp.CognitiveAIApis.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public class EndpointWithVersion : IWithVersion
    {
        private Dictionary<string, object> _innerDict;

        public EndpointWithVersion()
        {
        }

        public EndpointWithVersion(Dictionary<string, object> innerDict)
        {
            _innerDict = innerDict;
        }

        public IWithMethod AndMethod(string method)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(method), method);
            return new RestMethod(_innerDict);
        }

        public IWithMethod AndSubscriptionKey(string subscriptionKey)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(subscriptionKey), subscriptionKey);
            return new RestMethod(_innerDict);
        }
    }
}
