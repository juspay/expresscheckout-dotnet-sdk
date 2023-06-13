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
        public JuspayResponse CreateJuspayResponse(int statusCode, HttpResponseHeaders headers, bool isSuccessStatusCode, string content)
        {
            this.ResponseBase = new JuspayResponseBase(statusCode, headers, isSuccessStatusCode);
            this.RawContent = content;
            // this.Response = JsonConverter.ConvertJsonToDictionary(content); to prevent unnecessary computation
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
               try 
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
                        return (T) Response[key];
                    }
                
                    return (T)Response[key];
               }
                catch (Exception ) 
                {
                    return default(T);
                }
            }
            return default(T);
        }

        protected List<T> GetObjectList<T>(string key) where T : IJuspayResponseEntity, new()
        {
            List<T> listObj = new List<T>();
            if (Response.ContainsKey(key)) {
               try
               {
                    foreach (object item in (Response[key] as List<Dictionary<string, object>>)) {
                        T obj = new T();
                        obj.Response = item as Dictionary<string, object>;
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

        protected T GetObject<T> (string key) where T : IJuspayResponseEntity, new()
        {
            T obj = new T();
            if (Response.ContainsKey(key) && Response[key] != null)
            {
                try {
                     obj.Response = Response[key] as Dictionary<string, object>;
                    return obj;
                }
                catch (Exception)
                {
                    return default (T);
                }
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
            response.Response = JsonConvertToJuspayTargetType.ConvertJsonToDictionary<T>(value);
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

        public static Dictionary<string, object> ConvertJObjectToDictionary(JObject jsonObject)
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
                    dictionary[property.Name] = ConvertJValueToBasicType((JValue)value);
                }
            }

            return dictionary;
        }

        public static List<dynamic> ConvertJArrayToList(JArray jsonArray)
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

        public static object ConvertJValueToBasicType(JValue jValue)
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

    public static class JsonConvertToJuspayTargetType
    {
        public static Dictionary<string, object> ConvertJsonToDictionary<T>(string json) where T : IJuspayResponseEntity, new()
        {
            var jsonObject = JsonConvert.DeserializeObject<JObject>(json);
            return ConvertJObjectToDictionary(jsonObject, typeof(T));
        }

        private static Dictionary<string, object> ConvertJObjectToDictionary(JObject jsonObject, Type targetType)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var property in jsonObject.Properties())
            {
                var propertyName = property.Name;
                var propertyValue = property.Value;

                var propertyInfo = targetType.GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName == propertyName);
                if (propertyInfo != null)
                {
                    var convertedValue = ConvertJTokenToValue(propertyValue, propertyInfo.PropertyType);
                    dictionary[propertyName] = convertedValue;
                }
                else
                {
                    var value = property.Value;

                    if (value.Type == JTokenType.Object)
                    {
                        dictionary[property.Name] = JsonConverter.ConvertJObjectToDictionary((JObject)value);
                    }
                    else if (value.Type == JTokenType.Array)
                    {
                        
                        dictionary[property.Name] = JsonConverter.ConvertJArrayToList((JArray)value);
                    }
                    else
                    {
                        dictionary[property.Name] = JsonConverter.ConvertJValueToBasicType((JValue)value);
                    }
                }
            }

            return dictionary;
        }

        private static object ConvertJTokenToValue(JToken jToken, Type targetType)
        {
            if (targetType == typeof(Dictionary<string, object>))
            {
                return JsonConverter.ConvertJObjectToDictionary(jToken as JObject);
            }
            else if (jToken.Type == JTokenType.Object)
            {
                var objectType = targetType;
                return ConvertJObjectToDictionary((JObject)jToken, targetType);
            }
            else if (jToken.Type == JTokenType.Array)
            {

                return ConvertJArrayToList((JArray)jToken, targetType);

            }
            else
            {
                return JsonConverter.ConvertJValueToBasicType((JValue)jToken);
            }
        }


        private static object ConvertJValueToBasicTargetType(JValue jValue, Type targetType)
        {
            if (jValue.Value == null)
            {
                return null;
            }
            else
            {
                return Convert.ChangeType(jValue.Value, targetType);
            }
        }


        private static dynamic ConvertJArrayToList(JArray jsonArray, Type targetType)
        {
            dynamic list = Activator.CreateInstance(targetType);
            Type innerType = targetType.GetElementType() ?? targetType.GetGenericArguments()[0];
            if (typeof(JuspayResponse).IsAssignableFrom(innerType)) {
                list = new List<Dictionary<string, object>>();
            }
            foreach (var item in jsonArray)
            {
                if (item.Type == JTokenType.Object)
                {
                    dynamic innerItem = ConvertJTokenToValue((JObject)item, innerType);
                    list.Add(innerItem);
                }
                else if (item.Type == JTokenType.Array)
                {
                    list.Add(ConvertJArrayToList((JArray)item, innerType));
                }
                else
                {
                    dynamic innerItem = JsonConverter.ConvertJValueToBasicType((JValue)item);
                    list.Add(innerItem);
                }
            }
            return list;
        }

    }
}
