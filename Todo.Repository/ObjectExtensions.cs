using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Todo.Repository
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object value)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };

            return JsonConvert.SerializeObject(value, settings);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string CsvToJson(this string value)
        {
            // Get lines.
            if (value == null) return null;
            string[] lines = value.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) throw new InvalidDataException("Must have header line.");

            // Get headers.
            string[] headers = lines.First().Split(new char[] { ',' });

            // Build JSON array.
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new char[] { ',' });
                if (fields.Length != headers.Length) throw new InvalidDataException("Field count must match header count.");

                var jsonElements = headers.Zip(fields, (header, field) => string.Format("{0}: {1}", header, field)).ToArray();
                string jsonObject = "{" + string.Format("{0}", string.Join(",", jsonElements)) + "}";
                if (i < lines.Length - 1)
                    jsonObject += ",";
                sb.AppendLine(jsonObject);
            }
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}
