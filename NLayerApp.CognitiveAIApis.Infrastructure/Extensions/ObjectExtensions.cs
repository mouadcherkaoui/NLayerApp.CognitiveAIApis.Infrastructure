using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure.Helpers
{
    public static class ObjectExtensions
    {
        public static TResult PipeTo<TSource, TResult>(this TSource source, 
            Func<TSource, TResult> destination) => destination.Invoke(source);
    }
}
