using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure
{
    public interface IRequest<TRequest>
    {
        TRequest RequestObject { get; }
    }
}
