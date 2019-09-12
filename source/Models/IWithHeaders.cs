using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public interface IWithHeaders
    {
        /// <summary>
        /// method to add content-type
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        IWithContentType AndContentType(string contentType);
        /// <summary>
        /// method to add parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IWithParameters AndParameters(Dictionary<string, string> parameters);
        TResponse ProcessRequest<TRequest, TResponse>();
    }
}
