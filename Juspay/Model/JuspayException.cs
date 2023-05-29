namespace Juspay
{
    using System;
    using System.Net;

    public class JuspayException : Exception
    {
        public JuspayException(string message)
            : base(message)
        {
        }

        public JuspayException(string message, Exception err)
            : base(message, err)
        {
        }

        public JuspayException(HttpStatusCode httpStatusCode, JuspayError JuspayError, string message)
            : base(message)
        {
            this.HttpStatusCode = httpStatusCode;
            this.JuspayError = JuspayError;
            this.JuspayResponse = JuspayError?.JuspayResponse;
        }

        public HttpStatusCode HttpStatusCode { get; set; }

        public JuspayError JuspayError { get; set; }

        public JuspayResponse JuspayResponse { get; set; }
    }
}
