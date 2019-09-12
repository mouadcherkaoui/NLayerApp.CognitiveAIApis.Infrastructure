using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public interface IWithParameters
    {
        /// <summary>
        /// method to add content-type
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        IWithContentType AndContentType(string contentType);
        /// <summary>
        /// method to add headers as dictionary
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        IWithHeaders AndHeaders(Dictionary<string, string> headers);

        TResponse ProcessRequest<TRequest, TResponse>();
    }
}
