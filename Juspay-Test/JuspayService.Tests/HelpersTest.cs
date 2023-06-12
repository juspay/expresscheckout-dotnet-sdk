using Xunit;
using System;
using System.Collections.Generic;
using Juspay;
using Newtonsoft.Json;

namespace JuspayTest
{
   
    public class InputEntityTest
    {
        class InputEntityObjectTest : JuspayEntity {
            public InputEntityObjectTest() : base() {}
        
            public InputEntityObjectTest(Dictionary<string, object> data) : base(data) {}
        
            [JsonProperty("basic_string_field")]
            public string StringField
            {
                get { return GetValue<string>("basic_string_field"); }
                set { SetValue("basic_string_field", value); }
            }

            [JsonProperty("basic_int_field")]
            public int IntField
            {
                get { return GetValue<int>("basic_int_field"); }
                set { SetValue("basic_int_field", value); }
            }

            [JsonProperty("basic_float_field")]
            public float FloatField
            {
                get { return GetValue<float>("basic_float_field"); }
                set { SetValue("basic_float_field", value); }
            }

            [JsonProperty("basic_double_field")]
            public double DoubleField
            {
                get { return GetValue<double>("basic_double_field"); }
                set { SetValue("basic_double_field", value); }
            }

            [JsonProperty("basic_bool_field")]
            public bool BoolField
            {
                get { return GetValue<bool>("basic_bool_field"); }
                set { SetValue("basic_bool_field", value); }
            }

            [JsonProperty("basic_dictionary_field")]
            public Dictionary<string, object> DictionaryField
            {
                get { return GetValue<Dictionary<string, object>>("basic_dictionary_field"); }
                set {  SetValue("basic_dictionary_field", value); }
            }

            [JsonProperty("basic_list_field")]
            public List<string> ListField
            {
                get { return GetValue<List<string>>("basic_list_field"); }
                set { SetValue("basic_list_field", value); }
            }

            [JsonProperty("object_a")]
            public ObjectA ObjectAField
            {
                get { return GetObject<ObjectA>("object_a"); }
                set { SetValue("object_a", value); }
            }

            [JsonProperty("list_object_field")]
            public List<ObjectA> ListObjectField
            {
                get { return GetObjectList<ObjectA>("list_object_field"); }
                set { SetValue("list_object_field", value); }
            }

            [JsonProperty("nested_list")]
            public List<List<string>> NestedListField
            {
                get { return GetValue<List<List<string>>>("nested_list"); }
                set { SetValue("nested_list", value ); }
            }
            
        }

        public class ObjectA : JuspayEntity
        {
            public ObjectA() : base() {}
            
            public ObjectA(Dictionary<string, object> data) : base(data) {}

            [JsonProperty("basic_string_field")]
            public string StringField
            {
                get { return GetValue<string>("basic_string_field"); }
                set { SetValue("basic_string_field", value); }
            }

            [JsonProperty("basic_int_field")]
            public int IntField
            {
                get { return GetValue<int>("basic_int_field"); }
                set { SetValue("basic_int_field", value); }
            }

            [JsonProperty("basic_float_field")]
            public float FloatField
            {
                get { return GetValue<float>("basic_float_field"); }
                set { SetValue("basic_float_field", value); }
            }

            [JsonProperty("basic_double_field")]
            public double DoubleField
            {
                get { return GetValue<double>("basic_double_field"); }
                set { SetValue("basic_double_field", value); }
            }

            [JsonProperty("basic_bool_field")]
            public bool BoolField
            {
                get { return GetValue<bool>("basic_bool_field"); }
                set { SetValue("basic_bool_field", value); }
            }

            [JsonProperty("basic_dictionary_field")]
            public Dictionary<string, object> DictionaryField
            {
                get { return GetValue<Dictionary<string, object>>("basic_dictionary_field"); }
                set {  SetValue("basic_dictionary_field", value); }
            }

            [JsonProperty("basic_list_field")]
            public List<string> ListField
            {
                get { return GetValue<List<string>>("basic_list_field"); }
                set {  SetValue("basic_list_field", value); }
            }

            [JsonProperty("object_b")]
            public ObjectB ObjectBField
            {
                get { return GetObject<ObjectB>("object_b"); }
                set {  SetValue("object_b", value); }
            }

        }

        public class ObjectB : JuspayEntity
        {
            public ObjectB() : base() {}
            
            public ObjectB(Dictionary<string, object> data) : base(data) {}

            [JsonProperty("basic_string_field")]
            public string StringField
            {
                get { return GetValue<string>("basic_string_field"); }
                set { SetValue("basic_string_field", value); }
            }

        }

        public static void TestInputEntity() {
            TestGetters();
            TestSetters();
            TestPartialSetters();
        }

        private static Dictionary<string, object> data;

        public static void setData() {
            data = new Dictionary<string, object>()
                {
                    { "basic_string_field", "Hello" },
                    { "basic_int_field", 123 },
                    { "basic_float_field", 3.14f },
                    { "basic_double_field", 3.14159 },
                    { "basic_bool_field", true },
                    { "basic_dictionary_field", new Dictionary<string, object> { { "key", "value" } } },
                    { "basic_list_field", new List<string> { "item1", "item2", "item3" } },
                    {
                        "nested_list",
                        new List<List<string>>
                        {
                            new List<string> { "Item 1 of List 1", "Item 2 of List 1", "Item 3 of List 1" },
                            new List<string> { "Item 1 of List 2", "Item 2 of List 2" },
                            new List<string> { "Item 1 of List 3", "Item 2 of List 3", "Item 3 of List 3", "Item 4 of List 3" }
                        }
                    },
                    {
                        "object_a",
                        new Dictionary<string, object>
                        {
                            { "basic_string_field", "Nested object" },
                            { "basic_int_field", 456 },
                            { "basic_list_field", new List<string> { "objectA 1", "objectA 2", "objectA 3" } },
                            {
                                "object_b",
                                new Dictionary<string, object>
                                {
                                    { "basic_string_field", "Nested nested object" }
                                }
                            }
                        }
                    },
                    {
                        "list_object_field",
                        new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "basic_string_field", "Item 1" },
                                { "basic_int_field", 111 },
                                { "basic_list_field", new List<int> { 1, 2, 3 } },
                                {
                                    "object_b",
                                    new Dictionary<string, object>
                                    {
                                        { "basic_string_field", "Nested nested object" }
                                    }
                                }

                            },
                            new Dictionary<string, object>
                            {
                                { "basic_string_field", "Item 2" },
                                { "basic_int_field", 222 },
                            },
                        }
                    }

                };
        }

        public static void TestGetters() {
            setData();
            InputEntityObjectTest InputObj = new InputEntityObjectTest(data);
            Assert.True(InputObj.StringField == "Hello");
            Assert.True(InputObj.IntField == 123);
            Assert.True(InputObj.FloatField == 3.14f);
            Assert.True(InputObj.DoubleField == 3.14159);
            Assert.True(InputObj.BoolField == true);
            Assert.True(((string)InputObj.DictionaryField["key"]) == "value");
            Assert.True(InputObj.ListField[0] == "item1");
            Assert.True(InputObj.NestedListField[0][0] == "Item 1 of List 1");
            Assert.True(InputObj.ObjectAField.IntField == 456);
            Assert.True(((int)InputObj.ObjectAField.Data["basic_int_field"]) == 456);
            Assert.True(InputObj.ObjectAField.ListField[0] == "objectA 1");
            Assert.True(((List<string>)InputObj.ObjectAField.Data["basic_list_field"])[0] == "objectA 1");
            Assert.True(InputObj.ObjectAField.ObjectBField.StringField == "Nested nested object");
            Assert.True(((string)InputObj.ObjectAField.ObjectBField.Data["basic_string_field"]) == "Nested nested object");
            Assert.True(InputObj.ListObjectField[0].IntField == 111);
        }
        public static void TestSetters()
        {
            setData();
            InputEntityObjectTest InputObj = new InputEntityObjectTest(data);
            InputObj.StringField = "Hello updated";
            Assert.True(InputObj.StringField == "Hello updated");
            Assert.True(((string)InputObj.Data["basic_string_field"]) == InputObj.StringField);
            InputObj.Data["basic_string_field"] = "Hello updated data";
            Assert.True(InputObj.StringField == "Hello updated data");
            Assert.True(((string)InputObj.Data["basic_string_field"]) == InputObj.StringField);
            InputObj.IntField = 321;
            Assert.True(InputObj.IntField == 321);
            Assert.True(((int)InputObj.Data["basic_int_field"]) == InputObj.IntField);
            InputObj.Data["basic_int_field"] = 456;
            Assert.True(InputObj.IntField == 456);
            Assert.True(((int)InputObj.Data["basic_int_field"]) == InputObj.IntField);
            InputObj.ObjectAField.IntField = 999;
            Assert.True(((int)((Dictionary<string, object>)InputObj.Data["object_a"])["basic_int_field"]) == 999);
            Assert.True(InputObj.ObjectAField.IntField == 999);
            Assert.True(((int)InputObj.ObjectAField.Data["basic_int_field"]) == 999);
            ((Dictionary<string, object>)InputObj.Data["object_a"])["basic_int_field"] = 111;
            Assert.True((int)(((Dictionary<string, object>)InputObj.Data["object_a"])["basic_int_field"]) == 111);
            Assert.True(((int)InputObj.ObjectAField.Data["basic_int_field"]) == 111);
            Assert.True(InputObj.ObjectAField.IntField == 111);
            InputObj.ListField[0] = "item1 updated"; 
            Assert.True(((List<string>)InputObj.Data["basic_list_field"])[0] == "item1 updated");
            Assert.True(InputObj.ListField[0] == "item1 updated");
            ((List<string>)InputObj.Data["basic_list_field"])[0] = "item1 data updated";
            Assert.True(((List<string>)InputObj.Data["basic_list_field"])[0] == "item1 data updated");
            Assert.True(InputObj.ListField[0] == "item1 data updated");
            InputObj.ListObjectField[0].IntField = 9999;
            Assert.True(((int)((List<Dictionary<string, object>>)InputObj.Data["list_object_field"])[0]["basic_int_field"]) == 9999);
            Assert.True(InputObj.ListObjectField[0].IntField == 9999);
            ((List<Dictionary<string, object>>)InputObj.Data["list_object_field"])[0]["basic_int_field"] = 555;
            Assert.True(((int)((List<Dictionary<string, object>>)InputObj.Data["list_object_field"])[0]["basic_int_field"]) == 555);
            Assert.True(InputObj.ListObjectField[0].IntField == 555);
        }
        public static void TestPartialSetters() {
            setData();
            InputEntityObjectTest InputObj = new InputEntityObjectTest(data);
            InputObj.ObjectAField = null;
            ObjectA InputObjectA = new ObjectA(
                new Dictionary<string, object>
                        {
                            { "basic_string_field", "updated nested object" },
                            { "basic_int_field", 100 },
                            { "basic_list_field", new List<string> { "updated objectA 1", "updated objectA 2", "updated objectA 3" } },
                            {
                                "object_b",
                                new Dictionary<string, object>
                                {
                                    { "basic_string_field", "updated nested nested object" }
                                }
                            }
                        }
                );
            Assert.Null(InputObj.Data["object_a"]);
            Assert.Null(InputObj.ObjectAField);
            InputObj.ObjectAField = InputObjectA;
            Assert.NotNull(InputObj.Data["object_a"]);
            Assert.True(((int)((Dictionary<string, object>)InputObj.Data["object_a"])["basic_int_field"]) == 100);
            Assert.True(InputObj.ObjectAField.IntField == 100);
            Assert.NotNull(InputObj.ObjectAField);
        }
    }

    public class ResponseEntityTest
    {
        class ResponseEntityObjectTest : JuspayResponse {
        
            [JsonProperty("basic_string_field")]
            public string StringField
            {
                get { return GetValue<string>("basic_string_field"); }
                set { SetValue("basic_string_field", value); }
            }

            [JsonProperty("basic_int_field")]
            public int IntField
            {
                get { return GetValue<int>("basic_int_field"); }
                set { SetValue("basic_int_field", value); }
            }

            [JsonProperty("basic_float_field")]
            public float FloatField
            {
                get { return GetValue<float>("basic_float_field"); }
                set { SetValue("basic_float_field", value); }
            }

            [JsonProperty("basic_double_field")]
            public double DoubleField
            {
                get { return GetValue<double>("basic_double_field"); }
                set { SetValue("basic_double_field", value); }
            }

            [JsonProperty("basic_bool_field")]
            public bool BoolField
            {
                get { return GetValue<bool>("basic_bool_field"); }
                set { SetValue("basic_bool_field", value); }
            }

            [JsonProperty("basic_dictionary_field")]
            public Dictionary<string, object> DictionaryField
            {
                get { return GetValue<Dictionary<string, object>>("basic_dictionary_field"); }
                set {  SetValue("basic_dictionary_field", value); }
            }

            [JsonProperty("basic_list_field")]
            public List<string> ListField
            {
                get { return GetValue<List<string>>("basic_list_field"); }
                set { SetValue("basic_list_field", value); }
            }

            [JsonProperty("object_a")]
            public ObjectA ObjectAField
            {
                get { return GetObject<ObjectA>("object_a"); }
                set { SetValue("object_a", value); }
            }

            [JsonProperty("list_object_field")]
            public List<ObjectA> ListObjectField
            {
                get { return GetObjectList<ObjectA>("list_object_field"); }
                set { SetValue("list_object_field", value); }
            }

            [JsonProperty("nested_list")]
            public List<List<string>> NestedListField
            {
                get { return GetValue<List<List<string>>>("nested_list"); }
                set { SetValue("nested_list", value ); }
            }
            
        }

        public class ObjectA : JuspayResponse
        {

            [JsonProperty("basic_string_field")]
            public string StringField
            {
                get { return GetValue<string>("basic_string_field"); }
                set { SetValue("basic_string_field", value); }
            }

            [JsonProperty("basic_int_field")]
            public int IntField
            {
                get { return GetValue<int>("basic_int_field"); }
                set { SetValue("basic_int_field", value); }
            }

            [JsonProperty("basic_float_field")]
            public float FloatField
            {
                get { return GetValue<float>("basic_float_field"); }
                set { SetValue("basic_float_field", value); }
            }

            [JsonProperty("basic_double_field")]
            public double DoubleField
            {
                get { return GetValue<double>("basic_double_field"); }
                set { SetValue("basic_double_field", value); }
            }

            [JsonProperty("basic_bool_field")]
            public bool BoolField
            {
                get { return GetValue<bool>("basic_bool_field"); }
                set { SetValue("basic_bool_field", value); }
            }

            [JsonProperty("basic_dictionary_field")]
            public Dictionary<string, object> DictionaryField
            {
                get { return GetValue<Dictionary<string, object>>("basic_dictionary_field"); }
                set {  SetValue("basic_dictionary_field", value); }
            }

            [JsonProperty("basic_list_field")]
            public List<string> ListField
            {
                get { return GetValue<List<string>>("basic_list_field"); }
                set {  SetValue("basic_list_field", value); }
            }

            [JsonProperty("object_b")]
            public ObjectB ObjectBField
            {
                get { return GetObject<ObjectB>("object_b"); }
                set {  SetValue("object_b", value); }
            }

        }

        public class ObjectB : JuspayResponse
        {

            [JsonProperty("basic_string_field")]
            public string StringField
            {
                get { return GetValue<string>("basic_string_field"); }
                set { SetValue("basic_string_field", value); }
            }

        }

        public static void TestResponseEntity() {
            TestGetters();
            TestSetters();
            TestPartialSetters();
        }

        private static string response;

        public static void setData() {
            response = "{ \"basic_string_field\": \"Hello\", \"basic_int_field\": 123, \"basic_float_field\": 3.14, \"basic_double_field\": 3.14159, \"basic_bool_field\": true, \"basic_dictionary_field\": { \"key\": \"value\" }, \"basic_list_field\": [ \"item1\", \"item2\", \"item3\" ], \"nested_list\": [ [ \"Item 1 of List 1\", \"Item 2 of List 1\", \"Item 3 of List 1\" ], [ \"Item 1 of List 2\", \"Item 2 of List 2\" ], [ \"Item 1 of List 3\", \"Item 2 of List 3\", \"Item 3 of List 3\", \"Item 4 of List 3\" ] ], \"object_a\": { \"basic_string_field\": \"Nested object\", \"basic_int_field\": 456, \"basic_list_field\": [ \"objectA 1\", \"objectA 2\", \"objectA 3\" ], \"object_b\": { \"basic_string_field\": \"Nested nested object\" } }, \"list_object_field\": [ { \"basic_string_field\": \"Item 1\", \"basic_int_field\": 111, \"basic_list_field\": [ \"1\", \"2\", \"3\" ], \"object_b\": { \"basic_string_field\": \"Nested nested object\" } }, { \"basic_string_field\": \"Item 2\", \"basic_int_field\": 222 } ] }";
        }

        public static void TestGetters() {
            setData();
            ResponseEntityObjectTest InputObj = JuspayResponse.FromJson<ResponseEntityObjectTest>(response);
            Assert.True(InputObj.StringField == "Hello");
            Assert.True(InputObj.IntField == 123);
            Assert.True(InputObj.FloatField == 3.14f);
            Assert.True(InputObj.DoubleField == 3.14159);
            Assert.True(InputObj.BoolField == true);
            Assert.True(InputObj.ListField[0] == "item1");
            Assert.True(InputObj.NestedListField[0][0] == "Item 1 of List 1");
            Assert.True(((string)InputObj.DictionaryField["key"]) == "value");
            Assert.True(InputObj.ObjectAField.IntField == 456);
            Assert.True(((int)InputObj.ObjectAField.Response["basic_int_field"]) == 456);
            Assert.True(InputObj.ObjectAField.ListField[0] == "objectA 1");
            Assert.True(((List<string>)InputObj.ObjectAField.Response["basic_list_field"])[0] == "objectA 1");
            Assert.True(InputObj.ObjectAField.ObjectBField.StringField == "Nested nested object");
            Assert.True(((string)InputObj.ObjectAField.ObjectBField.Response["basic_string_field"]) == "Nested nested object");
            Assert.True(InputObj.ListObjectField[0].IntField == 111);
        }
        public static void TestSetters()
        {
            setData();
            ResponseEntityObjectTest InputObj = JuspayResponse.FromJson<ResponseEntityObjectTest>(response);
            ((List<string>)InputObj.Response["basic_list_field"])[0] = "item1 data updated";
            Assert.True(((List<string>)InputObj.Response["basic_list_field"])[0] == "item1 data updated");
            Assert.True(InputObj.ListField[0] == "item1 data updated");
            InputObj.StringField = "Hello updated";
            Assert.True(InputObj.StringField == "Hello updated");
            Assert.True(((string)InputObj.Response["basic_string_field"]) == InputObj.StringField);
            InputObj.Response["basic_string_field"] = "Hello updated data";
            Assert.True(InputObj.StringField == "Hello updated data");
            Assert.True(((string)InputObj.Response["basic_string_field"]) == InputObj.StringField);
            InputObj.IntField = 321;
            Assert.True(InputObj.IntField == 321);
            Assert.True(((int)InputObj.Response["basic_int_field"]) == InputObj.IntField);
            InputObj.Response["basic_int_field"] = 456;
            Assert.True(InputObj.IntField == 456);
            Assert.True(((int)InputObj.Response["basic_int_field"]) == InputObj.IntField);
            InputObj.ObjectAField.IntField = 999;
            Assert.True(((int)((Dictionary<string, object>)InputObj.Response["object_a"])["basic_int_field"]) == 999);
            Assert.True(InputObj.ObjectAField.IntField == 999);
            Assert.True(((int)InputObj.ObjectAField.Response["basic_int_field"]) == 999);
            ((Dictionary<string, object>)InputObj.Response["object_a"])["basic_int_field"] = 111;
            Assert.True((int)(((Dictionary<string, object>)InputObj.Response["object_a"])["basic_int_field"]) == 111);
            Assert.True(((int)InputObj.ObjectAField.Response["basic_int_field"]) == 111);
            Assert.True(InputObj.ObjectAField.IntField == 111);
            InputObj.ListField[0] = "item1 updated"; 
            Assert.True(((List<string>)InputObj.Response["basic_list_field"])[0] == "item1 updated");
            Assert.True(InputObj.ListField[0] == "item1 updated");
            ((List<string>)InputObj.Response["basic_list_field"])[0] = "item1 data updated";
            Assert.True(((List<string>)InputObj.Response["basic_list_field"])[0] == "item1 data updated");
            Assert.True(InputObj.ListField[0] == "item1 data updated");
            InputObj.ListObjectField[0].IntField = 9999;
            Assert.True(((int)((List<Dictionary<string, object>>)InputObj.Response["list_object_field"])[0]["basic_int_field"]) == 9999);
            Assert.True(InputObj.ListObjectField[0].IntField == 9999);
            ((List<Dictionary<string, object>>)InputObj.Response["list_object_field"])[0]["basic_int_field"] = 555;
            Assert.True(((int)((List<Dictionary<string, object>>)InputObj.Response["list_object_field"])[0]["basic_int_field"]) == 555);
            Assert.True(InputObj.ListObjectField[0].IntField == 555);
        }
        public static void TestPartialSetters() {
            setData();
            ResponseEntityObjectTest InputObj = JuspayResponse.FromJson<ResponseEntityObjectTest>(response);
            InputObj.ObjectAField = null;
            ObjectA InputObjectA = JuspayResponse.FromJson<ObjectA>("{ \"basic_string_field\": \"Nested object\", \"basic_int_field\": 100, \"basic_list_field\": [ \"objectA 1\", \"objectA 2\", \"objectA 3\" ], \"object_b\": { \"basic_string_field\": \"Nested nested object\" } }");
            Assert.Null(InputObj.Response["object_a"]);
            Assert.Null(InputObj.ObjectAField);
            InputObj.ObjectAField = InputObjectA;
            Assert.NotNull(InputObj.Response["object_a"]);
            Assert.True(((int)((Dictionary<string, object>)InputObj.Response["object_a"])["basic_int_field"]) == 100);
            Assert.True(InputObj.ObjectAField.IntField == 100);
            Assert.NotNull(InputObj.ObjectAField);
        }
    }

}