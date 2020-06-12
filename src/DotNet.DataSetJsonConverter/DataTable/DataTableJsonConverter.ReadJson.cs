using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataTableJsonConverter
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            
            JObject jObject = JObject.Load(reader);
            if (jObject == null) throw new Exception();

            var table = new DataTable();

            if (jObject["CaseSensitive"] != null) table.CaseSensitive = jObject.Value<bool>("CaseSensitive");

            if (jObject["TableName"] != null) table.TableName = jObject.Value<string>("TableName");

            if (jObject["DisplayExpression"] != null) table.DisplayExpression = jObject.Value<string>("DisplayExpression");

            if (jObject["MinimumCapacity"] != null) table.MinimumCapacity = jObject.Value<int>("MinimumCapacity");

            if (jObject["Namespace"] != null) table.Namespace = jObject.Value<string>("Namespace");

            if(!jObject.ContainsKey("Columns")) throw new Exception("Columns 为空");
            var columnJsonConverter = new DataColumnJsonConverter(_level);
            foreach (var jToken in jObject["Columns"]!)
            {
                var colReader = jToken.CreateReader();
                var column = (DataColumn)columnJsonConverter.ReadJson(colReader, typeof(DataColumn), null, serializer);
                table.Columns.Add(column);
            }

            if (jObject["PrimaryKeys"] != null)
            {
                var pkCols = new List<DataColumn>();
                foreach (JValue jPrimaryKey in jObject["PrimaryKeys"])
                {
                    var colName = jPrimaryKey.ToString();
                    if (table.Columns.Contains(colName))
                    {
                        pkCols.Append(table.Columns[colName]);
                    }
                    else
                    {
                        // todo
                        throw new Exception("错误的PrimaryKeys");
                    }
                }

                table.PrimaryKey = pkCols.ToArray();
            }

            if (jObject["Rows"] != null)
            {
                foreach (var jToken in jObject["Rows"])
                {
                    var row = table.NewRow();
                    foreach (DataColumn col in table.Columns)
                    {
                        var rowValue = jToken[col.ColumnName];

                        if (rowValue != null) row[col.ColumnName] = rowValue.ToObject<object>();
                    }

                    table.Rows.Add(row);
                }
            }

            return table;
        }
    }
}