using System;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataColumnJsonConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var column = new DataColumn();

            JToken jToken = null;
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
                    ? typeof(Byte[])
                    : Type.GetType(string.Concat("System.", dataTypeStr));
            }

            if (!column.AutoIncrement)
            {
                jToken = jObject.SelectToken("DefaultValue");
                if (jToken != null && jToken.Type != JTokenType.Null)
                {
                    switch (Type.GetTypeCode(column.DataType))
                    {
                        case TypeCode.Boolean:
                            column.DefaultValue = Convert.ToBoolean(jToken);
                            break;
                        case TypeCode.Byte:
                            // todo
                            break;
                        case TypeCode.Char:
                            column.DefaultValue = Convert.ToChar(jToken);
                            break;
                        case TypeCode.DateTime:
                            column.DefaultValue = Convert.ToDateTime(jToken);
                            break;
                        case TypeCode.Decimal:
                            column.DefaultValue = Convert.ToDecimal(jToken);
                            break;
                        case TypeCode.Double:
                            column.DefaultValue = Convert.ToDouble(jToken);
                            break;
                        case TypeCode.Int32:
                            column.DefaultValue = Convert.ToInt32(jToken);
                            break;
                        case TypeCode.Int64:
                            column.DefaultValue = Convert.ToInt64(jToken);
                            break;
                        case TypeCode.String:
                            column.DefaultValue = Convert.ToString(jToken);
                            break;
                        default:
                            throw new Exception("不支持的类型");
                    }
                }
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


            return column;
        }
    }
}