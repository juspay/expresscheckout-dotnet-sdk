namespace Juspay
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JuspayEntity : IJuspayEntity
    {

        [JsonIgnore]
        public JuspayResponse? JuspayResponse { get; set; }

        public static T FromJson<T>(JuspayResponse value) where T : IJuspayEntity
        {
            T? response =  JsonConvert.DeserializeObject<T>(value.Content);
            if (response != null) response.JuspayResponse = value;
            else throw new Exception($"Deserialization Failed for type {typeof(T)}");
            return response;
        }
        public static T FromJson<T>(string value) where T : IJuspayEntity
        {
            T? response =  JsonConvert.DeserializeObject<T>(value);
            if (response != null) return response;
            throw new Exception($"Deserialization Failed for type {typeof(T)}");
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

        public string ToJson()
        {
            return JsonConvert.SerializeObject(
                this,
                Formatting.Indented);
        }


        private object? GetIdString()
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
    }
}
