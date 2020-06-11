using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataTableJsonConverter
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            if (jObject == null) throw new Exception();

            DataTable table = new DataTable();

            if (jObject["CaseSensitive"] != null) table.CaseSensitive = jObject.Value<bool>("CaseSensitive");

            if (jObject["TableName"] != null) table.TableName = jObject.Value<string>("TableName");

            if (jObject["DisplayExpression"] != null) table.DisplayExpression = jObject.Value<string>("DisplayExpression");

            if (jObject["MinimumCapacity"] != null) table.MinimumCapacity = jObject.Value<int>("MinimumCapacity");

            if (jObject["Namespace"] != null) table.Namespace = jObject.Value<string>("Namespace");

            // todo check null
            foreach (JObject colObj in jObject["Columns"])
            {
                var    column = new DataColumn();
                JToken jToken = null;
                jToken = colObj.SelectToken("AutoIncrementSeed");
                if (jToken != null) column.AutoIncrementSeed = jToken.Value<long>();

                jToken = colObj.SelectToken("AutoIncrementStep");
                if (jToken != null) column.AutoIncrementStep = jToken.Value<long>();

                jToken = colObj.SelectToken("AllowDBNull");
                if (jToken != null) column.AllowDBNull = jToken.Value<bool>();

                jToken = colObj.SelectToken("AutoIncrement");
                if (jToken != null) column.AutoIncrement = jToken.Value<bool>();

                jToken = colObj.SelectToken("Caption");
                if (jToken != null) column.Caption = jToken.Value<string>();

                jToken = colObj.SelectToken("ColumnName");
                if (jToken != null) column.ColumnName = jToken.Value<string>();

                jToken = colObj.SelectToken("DataType");
                if (jToken != null)
                {
                    var dataTypeStr = jToken.Value<string>();
                    column.DataType = dataTypeStr == "Byte[]"
                        ? typeof(Byte[])
                        : Type.GetType(string.Concat("System.", dataTypeStr));
                }

                if (!column.AutoIncrement)
                {
                    jToken = colObj.SelectToken("DefaultValue");
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


                jToken = colObj.SelectToken("MaxLength");
                if (jToken != null) column.MaxLength = jToken.Value<int>();

                jToken = colObj.SelectToken("Namespace");
                if (jToken != null) column.Namespace = jToken.Value<string>();

                jToken = colObj.SelectToken("Prefix");
                if (jToken != null) column.Prefix = jToken.Value<string>();

                jToken = colObj.SelectToken("ReadOnly");
                if (jToken != null) column.ReadOnly = jToken.Value<bool>();

                jToken = colObj.SelectToken("Unique");
                if (jToken != null) column.Unique = jToken.Value<bool>();

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
                foreach (JToken jRow in jObject["Rows"])
                {
                    DataRow row = table.NewRow();
                    foreach (DataColumn col in table.Columns)
                    {
                        var rowValue = jRow[col.ColumnName];

                        if (rowValue != null) row[col.ColumnName] = rowValue.ToObject<object>();
                    }

                    table.Rows.Add(row);
                }
            }

            return table;
        }
    }
}