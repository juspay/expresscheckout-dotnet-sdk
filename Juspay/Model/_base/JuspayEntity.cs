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
        Dictionary<string, object> Data { get; set; }
    }

    public class JuspayEntity : IJuspayEntity
    {
        public JuspayEntity() {}
    
        private Dictionary<string, object> data = new Dictionary<string, object>();

        public JuspayEntity(Dictionary<string, object> data) {
            this.Data = data;
        }
        
        [JsonIgnore]
        public Dictionary<string, object> Data
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

        protected T GetValue<T>(string key)
        {
            if (Data.ContainsKey(key))
            {
                try
                {
                    return (T)Data[key];
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            return default(T);
        }


        protected List<T> GetObjectList<T>(string key) where T : IJuspayEntity, new()
        {
            List<T> listObj = new List<T>();
            if (Data.ContainsKey(key)) {
                try
                {
                    foreach (Dictionary<string,object> item in (Data[key] as List<Dictionary<string,object>>)) {
                        T obj = new T();
                        obj.Data = item;
                        listObj.Add(obj);
                    }
                    return listObj;
                }
                catch (Exception)
                {
                    return default(List<T>);
                }
            }
            return default(List<T>);
        }

        protected T GetObject<T>(string key) where T : IJuspayEntity, new()
        {
            T obj = new T();

            if (Data.ContainsKey(key) && Data[key] != null) {
                try {
                    obj.Data = Data[key] as Dictionary<string, object>;
                    return obj;
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            return default(T);
        }
        
        protected void SetValue<T>(string key, T value)
        {
            if (value is JuspayEntity) {
                data[key] = ((IJuspayEntity)value).Data;
            } else {
                data[key] = value;
            }
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
