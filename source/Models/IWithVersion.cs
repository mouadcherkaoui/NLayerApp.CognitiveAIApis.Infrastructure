using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public interface IWithVersion
    {
        /// <summary>
        /// method to add Ocp-Apim-Subscription-Key to the headers collection
        /// </summary>
        /// <param name="SubscriptionKey"></param>
        /// <returns></returns>
        IWithMethod AndSubscriptionKey(string SubscriptionKey);
        /// <summary>
        /// method to specify the request method GET, POST, PUT, PATCH, DELETE
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IWithMethod AndMethod(string method);

    }
}
