namespace Juspay
{
    using System;
    using System.Collections.Generic;

    public class JuspayException : Exception
    {
        public JuspayException(string message)
            : base(message)
        {
            JuspayEnvironment.log.Error(message);
        }

        public JuspayException(string message, Exception err)
            : base(message, err)
        {
            JuspayEnvironment.SerializedLog(new Dictionary<string, string> { {"message", message}, { "error", err.ToString() } }, JuspayEnvironment.JuspayLogLevel.Error);
        }

        public JuspayException(int httpStatusCode, JuspayError JuspayError, JuspayResponse juspayResponse, string message)
            : base(message)
        {
            this.HttpStatusCode = httpStatusCode;
            this.JuspayError = JuspayError;
            this.JuspayResponse = juspayResponse;
            JuspayEnvironment.SerializedLog(new Dictionary<string, string> { {"message", message}, { "error", JuspayError?.ToString() }, { "juspay_response", JuspayResponse.RawContent }, { "http_status_code", httpStatusCode.ToString() } }, JuspayEnvironment.JuspayLogLevel.Error);
        }

        public int HttpStatusCode { get; set; }

        public JuspayError JuspayError { get; set; }

        public JuspayResponse JuspayResponse { get; set; }
    }

    public class AuthorizationException : JuspayException
    {
        public AuthorizationException(int httpStatusCode, JuspayError Error, JuspayResponse juspayResponse) : base(httpStatusCode, Error, juspayResponse, "AUTHORIZATION ERROR") {}
    }

    public class AuthenticationException : JuspayException
    {
        public AuthenticationException(int httpStatusCode, JuspayError Error, JuspayResponse juspayResponse) : base(httpStatusCode, Error, juspayResponse, "AUTHENTICATION ERROR check your API KEY") {}
    }

    public class InvalidRequestException : JuspayException
    {
        public InvalidRequestException(int httpStatusCode, JuspayError Error, JuspayResponse juspayResponse) : base(httpStatusCode, Error, juspayResponse, "INVALID REQUEST") {}
    }

    public class ValidationException : JuspayException
    {
        public ValidationException(string message) : base(message) {}
    }

    public class JWTException : JuspayException
    {
        public JWTException(string message) : base(message) {}  
    }

    public class JWKException : JuspayException
    {
        public JWKException(string message) : base(message) {}
    }

    public class JWEException : JuspayException
    {
        public JWEException(string message) : base(message) {}
    }

    public class JWSException : JuspayException
    {
        public JWSException(string message) : base(message) {}
    }
    
}
