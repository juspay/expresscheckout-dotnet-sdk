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
            T response =  JsonConvert.DeserializeObject<T>(value);
            if (response != null) return response;
            throw new JuspayException($"Deserialization Failed for type {typeof(T)}");
        }

        protected T GetValue<T>(string key)
        {
            if (Data.ContainsKey(key))
            {
                return (T)Data[key];
            }
            return default(T);
        }

        protected List<T> GetList<T>(string key)
        {
            List<T> listObj = new List<T>();
            if (Data.ContainsKey(key)) {
                foreach (T item in (Data[key] as List<T>)) {
                    listObj.Add((T)item);
                }
                return listObj;
            }
            return default(List<T>);

        }

        protected List<T> GetObjectList<T>(string key) where T : IJuspayEntity, new()
        {
            List<T> listObj = new List<T>();
            if (Data.ContainsKey(key)) {
                foreach (object item in (Data[key] as List<object>)) {
                    T obj = new T();
                    obj.Data = item as Dictionary<string, object>;
                    listObj.Add(obj);
                }
                return listObj;
            }
            return default(List<T>);
        }

        protected T GetObject<T>(string key) where T : IJuspayEntity, new()
        {
            T obj = new T();

            if (Data.ContainsKey(key)) {
                obj.Data = Data[key] as Dictionary<string, object>;
                return obj;
            }
            return default(T);
        }
        protected void SetValue<T>(string key, T value)
        {
            data[key] = value;
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
