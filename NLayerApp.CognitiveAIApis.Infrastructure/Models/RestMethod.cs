using NLayerApp.CognitiveAIApis.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public class RestMethod : IWithMethod
    {
        Dictionary<string, object> _innerDict;
        public RestMethod(Dictionary<string, object> innerDict)
        {
            _innerDict = innerDict;
        }
        public TResponse ProcessRequest<TRequest, TResponse>(TRequest request)
        {
            throw new NotImplementedException();
        }

        public IWithContentType WithContentType(string contentType)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(contentType), contentType);
            return new MethodWithContentType(_innerDict);
        }

        public IWithHeaders WithHeaders(Dictionary<string, string> headers)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(headers), headers);
            return new MethodWithHeaders(_innerDict);
        }

        public IWithParameters WithParameters(Dictionary<string, string> parameters)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(parameters), parameters);
            return new MethodWithParameters(_innerDict);
        }
    }

}
