using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.CognitiveAIApis.Infrastructure
{
    public interface IRequestHandler<TRequest, TResult> where TRequest: class // IRequest<TRequest>
    {
        Task<TResult> HandleRequestAsync(TRequest request);
    }
}
