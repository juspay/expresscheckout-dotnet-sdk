namespace Juspay
{
    using System.Net;
    using System.Net.Http.Headers;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System;
    public class JuspayResponse : IJuspayResponseEntity
    {
        public Dictionary<string, object> Response { get; set; }

        public JuspayResponseBase ResponseBase { get; set; }
        public JuspayResponse CreateJuspayResponse(HttpStatusCode statusCode, HttpResponseHeaders headers, bool isSuccessStatusCode, string content)
        {
            this.ResponseBase = new JuspayResponseBase(statusCode, headers, isSuccessStatusCode);
            this.RawContent = content;
            this.Response = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
            return this;
        }

        public string RawContent { get; set; }

        public static T FromJson<T>(string value) where T : IJuspayResponseEntity
        {
            T response =  JsonConvert.DeserializeObject<T>(value);
            if (response != null) {
                response.Response = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);
                return response;
            }
            throw new JuspayException($"Deserialization Failed for type {typeof(T)}");
        }

        public void PopulateObject() {
            JsonConvert.PopulateObject(this.RawContent, this);
        }
    
        public T FromJson<T>() where T : IJuspayResponseEntity {
            if (this.RawContent == null) throw new JuspayException($"Deserialization Failed for type {typeof(T)}");
            T response = FromJson<T>(this.RawContent);
            if (this.ResponseBase != null) response.ResponseBase = this.ResponseBase;
            if (this.RawContent != null) response.RawContent = this.RawContent;
            return response;
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
            if (this.Response != null) {
                return JsonConvert.SerializeObject(this.Response, Formatting.Indented);
            }
            return JsonConvert.SerializeObject(
                this,
                Formatting.Indented);
        }
    }
}
