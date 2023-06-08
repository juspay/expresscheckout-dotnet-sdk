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
                if (Response[key] is Int64 && typeof(T) == typeof(int))
                {
                    return (T)(object)Convert.ToInt32(Response[key]);
                }
                else if (Response[key] is double && typeof(T) == typeof(float))
                {
                    return (T)(object)Convert.ToSingle(Response[key]);
                }
                else if (Response[key] is double && typeof(T) == typeof(double))
                {
                    return (T)(object)Response[key];
                }
                else if (Response[key] is float && typeof(T) == typeof(double)) {
                    return (T)(object)Convert.ToDouble(Response[key].ToString());
                }
                else if (Response[key] is List<object> inputList) {
                    Type targetType = typeof(T);
                    Type innerListType = targetType.GetGenericArguments()[0];
                    dynamic result = Activator.CreateInstance(targetType);
                    foreach (object item in inputList)
                    {
                        dynamic convertedItem = ConvertToObjectListHelper(innerListType, item);
                        result.Add(convertedItem);
                    }
                    Response[key] = result;
                    return (T) Response[key];
                }
                return (T)Response[key];
            }
            return default(T);
        }


        private static dynamic ConvertToObjectListHelper(Type innerListType, object item)
        {
            Type[] genericArguments = innerListType.GetGenericArguments();
            if (genericArguments.Length == 0)
            {
                return item;
            }
            dynamic innerList = Activator.CreateInstance(innerListType);
            if (item is List<object> nestedList)
            {
                foreach (object nestedItem in nestedList)
                {
                    dynamic convertedItem = ConvertToObjectListHelper(innerListType.GetGenericArguments()[0], nestedItem);
                    innerList.Add(convertedItem);
                }
            }
            else
            {
                throw new ArgumentException("Invalid object type in the input list.");
            }
            return innerList;
        }

        protected List<T> GetObjectList<T>(string key) where T : IJuspayResponseEntity, new()
        {
            List<T> listObj = new List<T>();
            if (Response.ContainsKey(key)) {
                foreach (object item in (Response[key] as List<Dictionary<string, object>>)) {
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
            if (Response.ContainsKey(key) && Response[key] != null)
            {
                obj.Response = Response[key] as Dictionary<string, object>;
                return obj;
            }
           return default(T);
        }

        protected void SetValue<T>(string key, T value)
        {
            if (value is JuspayResponse)
            {
                Response[key] = ((IJuspayResponseEntity)value).Response;
            }
            else
            {
                Response[key] = value;
            }
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
                    if (((JArray)value).Count > 0 && ((JArray)value)[0].Type == JTokenType.Object) {
                        var dictList = new List<Dictionary<string, object>>();
                        foreach(var item in ((JArray)value))
                        {
                            dictList.Add(ConvertJObjectToDictionary((JObject)item));
                        }
                        dictionary[property.Name] = dictList;
                    }
                    else
                    {
                        dictionary[property.Name] = ConvertJArrayToList((JArray)value);
                    }
                }
                else
                {
                    dictionary[property.Name] = ConvertJValueToBasicType((JValue)value);
                }
            }

            return dictionary;
        }

        private static List<dynamic> ConvertJArrayToList(JArray jsonArray)
        {
            var list = new List<dynamic>();
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
                    list.Add(ConvertJValueToBasicType((JValue)item));
                }
            }
            return list;
        }

        private static object ConvertJValueToBasicType(JValue jValue)
        {
            object value;
            switch (jValue.Type)
            {
                case JTokenType.Integer:
                    value = jValue.Value<int>();
                    break;
                case JTokenType.Float:
                    value = jValue.Value<float>();
                    break;
                case JTokenType.String:
                    value = jValue.Value<string>();
                    break;
                case JTokenType.Boolean:
                    value = jValue.Value<bool>();
                    break;
                case JTokenType.Null:
                    value = null;
                    break;
                default:
                    value = jValue.Value;
                    break;
            }

            return value;
        }

    }
}
