namespace Juspay
{
    using System.Net;
    using System.Net.Http.Headers;
    using System.Linq;
    using System.Reflection;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System;
    public class JuspayResponse : IJuspayResponseEntity
    {
        public dynamic response { get; set; } = new Dictionary<string, dynamic>();

        public JuspayResponseBase ResponseBase { get; set; }
        public JuspayResponse() {}
        public JuspayResponse(int statusCode, HttpResponseHeaders headers, bool isSuccessStatusCode, string content)
        {
            this.ResponseBase = new JuspayResponseBase(statusCode, headers, isSuccessStatusCode);
            this.RawContent = content;
            if (content == "")
            {
                this.Response = "";
            }
            else
            {
                this.Response = JObject.Parse(content);
            }
        }

        public dynamic FromJson(string value)
        {
            this.Response = JObject.Parse(value);
            return Response;
        }

        public string RawContent { get; set; }

        public dynamic Response
        {
            get { return response; }
            set { response = value; }
        }

        public override string ToString()
        {
            return string.Format(
                "<{0}@{1} id={2}> JSON: {3}",
                this.GetType().FullName,
                RuntimeHelpers.GetHashCode(this),
                this.GetIdString(),
                this.ToJson());
        }


        private object GetIdString()
        {
            foreach (var property in this.GetType().GetTypeInfo().DeclaredProperties)
            {
                if (property.Name == "Id")
                {
                    return property.GetValue(this);
                }
            }

            return null;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(
                this,
                Formatting.Indented);
        }
    }
}
