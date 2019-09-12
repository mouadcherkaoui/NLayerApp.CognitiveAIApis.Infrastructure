using NLayerApp.CognitiveAIApis.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public class MethodWithHeaders : IWithHeaders
    {
        private Dictionary<string, object> _innerDict;

        public MethodWithHeaders(Dictionary<string, object> innerDict)
        {
            _innerDict = innerDict;
        }

        /// <summary>
        /// method to add/specify content-type
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public IWithContentType AndContentType(string contentType)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(contentType), contentType);
            return new MethodWithContentType(_innerDict);
        }

        /// <summary>
        /// method to add parameters as a dictionary
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IWithParameters AndParameters(Dictionary<string, string> parameters)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(parameters), parameters);
            return new MethodWithParameters(_innerDict);
        }

        public TResponse ProcessRequest<TRequest, TResponse>()
        {
            throw new NotImplementedException();
        }
    }
}
