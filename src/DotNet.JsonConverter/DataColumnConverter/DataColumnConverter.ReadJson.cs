using System;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNet.JsonConverter
{
    public partial class DataColumnConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var column = new DataColumn();

            // ReSharper disable once RedundantAssignment
            JToken? jToken = null;
            jToken = jObject.SelectToken("AutoIncrementSeed");
            if (jToken != null) column.AutoIncrementSeed = jToken.Value<long>();

            jToken = jObject.SelectToken("AutoIncrementStep");
            if (jToken != null) column.AutoIncrementStep = jToken.Value<long>();

            jToken = jObject.SelectToken("AllowDBNull");
            if (jToken != null) column.AllowDBNull = jToken.Value<bool>();

            jToken = jObject.SelectToken("AutoIncrement");
            if (jToken != null) column.AutoIncrement = jToken.Value<bool>();

            jToken = jObject.SelectToken("Caption");
            if (jToken != null) column.Caption = jToken.Value<string>();

            jToken = jObject.SelectToken("ColumnName");
            if (jToken != null) column.ColumnName = jToken.Value<string>();

            jToken = jObject.SelectToken("DataType");
            if (jToken != null)
            {
                var dataTypeStr = jToken.Value<string>();
                column.DataType = dataTypeStr == "Byte[]"
                    ? throw new JsonException($"{nameof(DataTableConverter)} Error : 暂不支持该类型")
                    : Type.GetType(string.Concat("System.", dataTypeStr));
            }

            jToken = jObject.SelectToken("MaxLength");
            if (jToken != null) column.MaxLength = jToken.Value<int>();

            jToken = jObject.SelectToken("Namespace");
            if (jToken != null) column.Namespace = jToken.Value<string>();

            jToken = jObject.SelectToken("Prefix");
            if (jToken != null) column.Prefix = jToken.Value<string>();

            jToken = jObject.SelectToken("ReadOnly");
            if (jToken != null) column.ReadOnly = jToken.Value<bool>();

            jToken = jObject.SelectToken("Unique");
            if (jToken != null) column.Unique = jToken.Value<bool>();

            
            // 自增列不允许设置 DefaultValue
            // Cannot set AutoIncrement property for a column with DefaultValue set.
            if (!column.AutoIncrement) 
            {
                jToken = jObject.SelectToken("DefaultValue");
                if (jToken != null && jToken.Type != JTokenType.Null && jToken.Type != JTokenType.None)
                {
                    var typeCode = Type.GetTypeCode(column.DataType);
                    column.DefaultValue = Convert.ChangeType(jToken, typeCode);
                }
            }


            return column;
        }
    }
}