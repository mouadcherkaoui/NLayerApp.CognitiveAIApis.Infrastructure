using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NLayerApp.CognitiveAIApis.Infrastructure
{
    public class ResponseWrapper<TResponse>
    {
        public string StatusCode { get; set; }
        public bool IsSuccessfull { get; set; }
        public string ReasonPhrase { get; set; }
        public TResponse ResponseContent { get; set; }
        public IEnumerable<ValidationResult> ValidationResults { get; set; }
    }
}
