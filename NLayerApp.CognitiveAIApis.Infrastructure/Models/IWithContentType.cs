using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public interface IWithContentType
    {
        /// <summary>
        /// method to add parameters as a dictionary
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IWithParameters AndParameters(Dictionary<string, string> parameters);
        /// <summary>
        /// method to add headers as dictionary
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        IWithHeaders AndHeaders(Dictionary<string, string> headers);
        TResponse ProcessRequest<TRequest, TResponse>();
    }
}
