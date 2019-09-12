using NLayerApp.CognitiveAIApis.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{

    public class RestEndpoint : IRestEndpoint
    {
        Dictionary<string, object> _innerDict;
        public RestEndpoint(Dictionary<string, object> innerDict)
        {
            _innerDict = innerDict;
        }

        /// <summary>
        /// method to add/specify the endpoint version 
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public IWithVersion WithVersion(string version)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(version), version);
            return new EndpointWithVersion(_innerDict) { };
        }

        /// <summary>
        /// method to add/specify the operation method GET, POST, PUT, PATCH, DELETE
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IWithMethod WithMethod(string method)
        {
            _innerDict.AddOrReplaceKeyValuePair(nameof(method), method);
            return new RestMethod(_innerDict) { };
        }
    }

}
