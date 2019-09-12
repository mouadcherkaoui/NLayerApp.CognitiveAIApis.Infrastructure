using NLayerApp.CognitiveAIApis.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public class MethodWithParameters : IWithParameters
    {
        private Dictionary<string, object> _innerDict;

        public MethodWithParameters(Dictionary<string, object> innerDict)
        {
            _innerDict = innerDict;
        }

        /// <summary>
        /// method to add/specifry content-type
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public IWithContentType AndContentType(string contentType)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(contentType), contentType);
            return new MethodWithContentType(_innerDict);
        }

        /// <summary>
        /// method to add headers as a dictionary
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public IWithHeaders AndHeaders(Dictionary<string, string> headers)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(headers), headers);
            return new MethodWithHeaders(_innerDict);
        }

        public TResponse ProcessRequest<TRequest, TResponse>()
        {
            throw new NotImplementedException();
        }
    }

}
