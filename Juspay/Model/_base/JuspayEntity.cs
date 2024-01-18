namespace Juspay
{
    using System.Collections.Generic;
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Interface that identifies all entities returned by Juspay.
    /// </summary>
    public interface IJuspayEntity
    {
        Dictionary<string, dynamic> Data { get; set; }
    }

    public class JuspayEntity : IJuspayEntity
    {
        public JuspayEntity() {}
    
        private Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

        public JuspayEntity(Dictionary<string, dynamic> data) {
            this.Data = data;
        }
        
        [JsonIgnore]
        public Dictionary<string, dynamic> Data
        {
            get { return data; }
            set { data = value; }
        }

        public static T FromJson<T>(string value) where T : IJuspayEntity
        {
            T response =  JsonSerializer.Deserialize<T>(value);
            if (response != null) return response;
            throw new JuspayException($"Deserialization Failed for type {typeof(T)}");
        }
        public override string ToString()
        {
            return string.Format(
                "<{0}> JSON: {1}",
                this.GetType().FullName,
                this.ToJson());
        }

        public string ToJson()
        {
            if (Data != null && Data.Count != 0) {
                return JsonSerializer.Serialize(Data, new JsonSerializerOptions() { WriteIndented = true});
            }
            return JsonSerializer.Serialize(
                this,
                new JsonSerializerOptions() { WriteIndented = true });
        }
    }
}
