using Xunit;
using System;
using System.Collections.Generic;
using Juspay;
using System.Text.Json.Serialization;

namespace JuspayTest
{

    public class ResponseEntityTest
    {

        public static void TestResponseEntity()
        {
            TestGetters();
            TestSetters();
        }

        private static string response;

        public static void setData() {
            response = "{ \"basic_string_field\": \"Hello\", \"basic_int_field\": 123, \"basic_float_field\": 3.14, \"basic_double_field\": 3.14159, \"basic_bool_field\": true, \"basic_object_field\": { \"key\": \"value\" }, \"basic_list_field\": [ \"item1\", \"item2\", \"item3\" ], \"nested_list\": [ [ \"Item 1 of List 1\", \"Item 2 of List 1\", \"Item 3 of List 1\" ], [ \"Item 1 of List 2\", \"Item 2 of List 2\" ], [ \"Item 1 of List 3\", \"Item 2 of List 3\", \"Item 3 of List 3\", \"Item 4 of List 3\" ] ], \"object_a\": { \"basic_string_field\": \"Nested object\", \"basic_int_field\": 456, \"basic_list_field\": [ \"objectA 1\", \"objectA 2\", \"objectA 3\" ], \"object_b\": { \"basic_string_field\": \"Nested nested object\" } }, \"list_object_field\": [ { \"basic_string_field\": \"Item 1\", \"basic_int_field\": 111, \"basic_list_field\": [ \"1\", \"2\", \"3\" ], \"object_b\": { \"basic_string_field\": \"Nested nested object\" } }, { \"basic_string_field\": \"Item 2\", \"basic_int_field\": 222 } ], \"extra_field\": \"extra\", \"extra_list\": [1, 2, 3], \"extra_object\": { \"basic_nest\": \"hello\" } }";
        }

        public static void TestGetters() {
            setData();
            JuspayResponse InputObj = new JuspayResponse();
            InputObj.FromJson(response);
            Assert.True((string)InputObj.Response["basic_string_field"] == "Hello");
            Assert.True((int)InputObj.Response["basic_int_field"] == 123);
            Assert.True((float)InputObj.Response["basic_float_field"] == 3.14f);
            Assert.True((bool)InputObj.Response["basic_bool_field"] == true);
            Assert.True((string)InputObj.Response["basic_list_field"][0] == "item1");
            Assert.True((string)InputObj.Response["nested_list"][0][0] == "Item 1 of List 1");
            Assert.True(((string)InputObj.Response["basic_object_field"]["key"]) == "value");
            Assert.True((int)InputObj.Response["object_a"]["basic_int_field"] == 456);
            Assert.True((string)InputObj.Response["object_a"]["basic_list_field"][0] == "objectA 1");
            Assert.True((string)InputObj.Response["object_a"]["object_b"]["basic_string_field"] == "Nested nested object");
            Assert.True((int)InputObj.Response["list_object_field"][0]["basic_int_field"] == 111);
        }
        public static void TestSetters()
        {
            setData();
            JuspayResponse InputObj = new JuspayResponse();
            InputObj.FromJson(response);
            InputObj.Response["basic_list_field"][0] = "item1 data updated";
            Assert.True((string)InputObj.Response["basic_list_field"][0] == "item1 data updated");
            InputObj.Response["basic_string_field"] = "Hello updated";
            Assert.True((string)InputObj.Response["basic_string_field"] == "Hello updated");
            InputObj.Response["basic_int_field"] = 321;
            Assert.True((int)InputObj.Response["basic_int_field"] == 321);
            InputObj.Response["object_a"]["basic_int_field"] = 999;
            Assert.True((int)InputObj.Response["object_a"]["basic_int_field"] == 999);
            InputObj.Response["basic_list_field"][0] = "item1 updated"; 
            Assert.True((string)InputObj.Response["basic_list_field"][0] == "item1 updated");
            InputObj.Response["list_object_field"][0]["basic_int_field"] = 9999;
            Assert.True((int)InputObj.Response["list_object_field"][0]["basic_int_field"] == 9999);
        }
    }

}