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
        public Dictionary<string, object> response { get; set; }

        public JuspayResponseBase ResponseBase { get; set; }
        public JuspayResponse CreateJuspayResponse(HttpStatusCode statusCode, HttpResponseHeaders headers, bool isSuccessStatusCode, string content)
        {
            this.ResponseBase = new JuspayResponseBase(statusCode, headers, isSuccessStatusCode);
            this.RawContent = content;
            this.Response = JsonConverter.ConvertJsonToDictionary(content);
            return this;
        }

        public string RawContent { get; set; }

        public Dictionary<string, object> Response
        {
            get { return response; }
            set { response = value; }
        }

        protected T GetValue<T>(string key)
        {
            if (Response.ContainsKey(key))
            {
                return (T)Response[key];
            }
            return default(T);
        }

        protected List<T> GetList<T>(string key)
        {
            List<T> listObj = new List<T>();
            if (Response.ContainsKey(key)) {
                foreach (T item in (Response[key] as List<T>)) {
                    listObj.Add((T)item);
                }
                return listObj;
            }
            return default(List<T>);

        }

        protected List<T> GetObjectList<T>(string key) where T : IJuspayResponseEntity, new()
        {
            List<T> listObj = new List<T>();
            if (Response.ContainsKey(key)) {
                foreach (object item in (Response[key] as List<object>)) {
                    T obj = new T();
                    obj.Response = item as Dictionary<string, object>;
                    listObj.Add(obj);
                }
                return listObj;
            }
            return default(List<T>);
        }

        protected T GetObject<T> (string key) where T : IJuspayResponseEntity, new()
        {
            T obj = new T();
            if (Response.ContainsKey(key))
            {
                obj.Response = Response[key] as Dictionary<string, object>;
                return obj;
            }
           return default(T);
        }
        protected void SetValue<T>(string key, T value)
        {
            Response[key] = value;
        }

        public static T FromJson<T>(string value) where T : IJuspayResponseEntity, new()
        {
            T response = new T();
            response.Response = JsonConverter.ConvertJsonToDictionary(value);
            return response;
            throw new JuspayException($"Deserialization Failed for type {typeof(T)}");
        }

    
        public T FromJson<T>() where T : IJuspayResponseEntity, new() {
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
    public static class JsonConverter
    {
        public static Dictionary<string, object> ConvertJsonToDictionary(string json)
        {
            var jsonObject = JsonConvert.DeserializeObject<JObject>(json);
            return ConvertJObjectToDictionary(jsonObject);
        }

        private static Dictionary<string, object> ConvertJObjectToDictionary(JObject jsonObject)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var property in jsonObject.Properties())
            {
                var value = property.Value;

                if (value.Type == JTokenType.Object)
                {
                    dictionary[property.Name] = ConvertJObjectToDictionary((JObject)value);
                }
                else if (value.Type == JTokenType.Array)
                {
                    dictionary[property.Name] = ConvertJArrayToList((JArray)value);
                }
                else
                {
                    dictionary[property.Name] = ((JValue)value).Value;
                }
            }

            return dictionary;
        }

        private static List<object> ConvertJArrayToList(JArray jsonArray)
        {
            var list = new List<object>();

            foreach (var item in jsonArray)
            {
                if (item.Type == JTokenType.Object)
                {
                    list.Add(ConvertJObjectToDictionary((JObject)item));
                }
                else if (item.Type == JTokenType.Array)
                {
                    list.Add(ConvertJArrayToList((JArray)item));
                }
                else
                {
                    list.Add(((JValue)item).Value);
                }
            }

            return list;
        }
    }
}
