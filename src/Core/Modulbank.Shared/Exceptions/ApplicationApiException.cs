using System;
using System.Net;

namespace Modulbank.Shared.Exceptions
{
    public class ApplicationApiException : Exception
    {
        public ApplicationApiException(HttpStatusCode code, object errors = null)
        {
            Errors = errors;
            Code = code;
        }

        public object Errors { get; set; }
        public HttpStatusCode Code { get; set; }
    }
}