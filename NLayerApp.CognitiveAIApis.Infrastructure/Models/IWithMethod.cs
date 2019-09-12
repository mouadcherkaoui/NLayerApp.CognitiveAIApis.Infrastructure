using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public interface IWithMethod
    {
        /// <summary>
        /// method to add content-type
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        IWithContentType WithContentType(string contentType);
        /// <summary>
        /// method to add required headers
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        IWithHeaders WithHeaders(Dictionary<string, string> headers);
        /// <summary>
        /// method to add parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IWithParameters WithParameters(Dictionary<string, string> parameters);
        TResponse ProcessRequest<TRequest, TResponse>(TRequest request);
    }
}
