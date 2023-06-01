namespace Juspay
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;

    [JsonObject(MemberSerialization.OptIn)]
    public class JuspayEntity : IJuspayEntity
    {
        public JuspayEntity() {}
    
        public JuspayEntity(Dictionary<string, object> data) {
            this.Data = data;
        }
        
        [JsonIgnore]
        public Dictionary<string, object> Data { get; set; }

        public static T FromJson<T>(string value) where T : IJuspayEntity
        {
            T response =  JsonConvert.DeserializeObject<T>(value);
            if (response != null) return response;
            throw new Exception($"Deserialization Failed for type {typeof(T)}");
        }

        public void PopulateObject(Dictionary<string, object> data) {
            string jsonString = JsonConvert.SerializeObject(data);
            JsonConvert.PopulateObject(jsonString, this);
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
            if (Data != null) {
                return JsonConvert.SerializeObject(Data, Formatting.Indented);
            }
            return JsonConvert.SerializeObject(
                this,
                Formatting.Indented);
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
    }
}
