using NLayerApp.CognitiveAIApis.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public class MethodWithContentType : IWithContentType
    {
        private Dictionary<string, object> _innerDict;

        public MethodWithContentType(Dictionary<string, object> innerDict)
        {
            _innerDict = innerDict;
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
