namespace Juspay
{
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Nodes;

    public interface IJuspayResponseEntity
    {
        dynamic response { get; set; }
        dynamic Response { get; set; }
        JuspayResponseBase ResponseBase { get; set; }
        string RawContent { get; set; }
    }
    public class JuspayResponse : IJuspayResponseEntity
    {
        public dynamic response { get; set; } = new Dictionary<string, dynamic>();

        public JuspayResponseBase ResponseBase { get; set; }
        public JuspayResponse() {}
        public JuspayResponse(int statusCode, HttpResponseHeaders headers, bool isSuccessStatusCode, string content)
        {
            this.ResponseBase = new JuspayResponseBase(statusCode, headers, isSuccessStatusCode);
            this.RawContent = content;
            try
            {
                this.Response = JsonSerializer.Deserialize<JsonObject>(content);
            }
            catch (JsonException)
            {
                this.Response = content;
            }
        }

        public dynamic FromJson(string value)
        {
            this.Response = JsonSerializer.Deserialize<JsonObject>(value);
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
            return JsonSerializer.Serialize(
                this,
                new JsonSerializerOptions() { WriteIndented = true,  Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        }
    }
}
