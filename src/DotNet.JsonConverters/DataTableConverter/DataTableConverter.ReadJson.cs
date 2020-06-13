using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;

namespace DotNet.JsonConverters
{
    public partial class DataTableConverter
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            #region 校验

            if (jObject == null) throw new JsonException($"{nameof(DataTableConverter)} Error : Json 转换失败");
            if (!jObject.ContainsKey("Columns")) throw new JsonException($"{nameof(DataTableConverter)} Error : 缺少 Columns 属性");
            if (jObject["Columns"] == null || jObject["Columns"]!.Type == JTokenType.Null) throw new JsonException($"{nameof(DataTableConverter)} Error : Columns 为空");
            if (jObject["Columns"]!.Type != JTokenType.Array) throw new JsonException($"{nameof(DataTableConverter)} Error : Columns 类型错误");
            if (!jObject.ContainsKey("PrimaryKeys")) throw new JsonException($"{nameof(DataTableConverter)} Error : 缺少 PrimaryKeys 属性");
            if (jObject["PrimaryKeys"] == null || jObject["PrimaryKeys"]!.Type == JTokenType.Null) throw new JsonException($"{nameof(DataTableConverter)} Error : PrimaryKeys 为空");
            if (jObject["PrimaryKeys"]!.Type != JTokenType.Array) throw new JsonException($"{nameof(DataTableConverter)} Error : PrimaryKeys 类型错误");
            if (!jObject.ContainsKey("Rows")) throw new JsonException($"{nameof(DataTableConverter)} Error : 缺少 Rows 属性");
            if (jObject["Rows"] == null || jObject["Rows"]!.Type == JTokenType.Null) throw new JsonException($"{nameof(DataTableConverter)} Error : Rows 为空");
            if (jObject["Rows"]!.Type != JTokenType.Array) throw new JsonException($"{nameof(DataTableConverter)} Error : Rows 类型错误");

            #endregion


            var table = new DataTable();

            if (jObject["CaseSensitive"] != null) table.CaseSensitive = jObject.Value<bool>("CaseSensitive");

            if (jObject["TableName"] != null) table.TableName = jObject.Value<string>("TableName");

            if (jObject["DisplayExpression"] != null) table.DisplayExpression = jObject.Value<string>("DisplayExpression");

            if (jObject["MinimumCapacity"] != null) table.MinimumCapacity = jObject.Value<int>("MinimumCapacity");

            if (jObject["Namespace"] != null) table.Namespace = jObject.Value<string>("Namespace");

            var columnJsonConverter = new DataColumnConverter(_level);

            #region Columns

            foreach (var jToken in jObject["Columns"]!)
            {
                var colReader = jToken.CreateReader();
                var column    = (DataColumn) columnJsonConverter.ReadJson(colReader, typeof(DataColumn), null, serializer);
                table.Columns.Add(column);
            }

            #endregion


            #region PrimaryKeys

            var pkCols = new List<DataColumn>();
            foreach (var jpkToken in jObject["PrimaryKeys"]!)
            {
                var colName = jpkToken.Value<string>();
                if (!table.Columns.Contains(colName)) throw new JsonException($"{nameof(DataTableConverter)} Error : PrimaryKeys 中 {colName} 不存在");
                pkCols.Add(table.Columns[colName]);
            }

            table.PrimaryKey = pkCols.ToArray();

            #endregion

            #region Rows

            foreach (var jToken in jObject["Rows"]!)
            {
                if (jToken.Type != JTokenType.Object) throw new JsonException($"{nameof(DataTableConverter)} Error : Rows Array 中对象类型错误，必须是 Json{nameof(JTokenType.Object)}");
                var row = table.NewRow();
                foreach (DataColumn col in table.Columns)
                {
                    var jValue = jToken[col.ColumnName];

                    var isDateTime = Type.GetTypeCode(col.DataType) == TypeCode.DateTime;
                    
                    if (jValue == null)
                    {
                        row[col.ColumnName] = col.DefaultValue;
                        continue;
                    }

                    if (isDateTime)
                    {
                        var dtReader = jValue.CreateReader();
                        row[col.ColumnName] = new DateTimeConverter(_style).ReadJson(dtReader, col.DataType, existingValue, serializer);
                        continue;
                    }

                    row[col.ColumnName] = jValue.ToObject<object>();
                }

                table.Rows.Add(row);
            }

            #endregion

            return table;
        }
    }
}