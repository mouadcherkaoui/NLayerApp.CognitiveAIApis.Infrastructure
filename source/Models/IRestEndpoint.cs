using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Models
{
    public interface IRestEndpoint
    {
        IWithVersion WithVersion(string version);
        IWithMethod WithMethod(string method);
    }
}
