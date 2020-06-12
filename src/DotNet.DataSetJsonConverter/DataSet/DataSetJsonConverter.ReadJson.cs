using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataSetJsonConverter
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            #region 校验

            if (jObject == null) throw new JsonException($"{nameof(DataSetJsonConverter)} Error : Json 转换失败");
            if (!jObject.ContainsKey("Tables")) throw new JsonException($"{nameof(DataSetJsonConverter)} Error : 缺少 Tables 属性");
            if (jObject["Tables"] == null || jObject["Tables"]!.Type == JTokenType.Null) throw new JsonException($"{nameof(DataSetJsonConverter)} Error : Tables 为空");
            if (jObject["Tables"]!.Type != JTokenType.Array) throw new JsonException($"{nameof(DataSetJsonConverter)} Error : Tables 类型错误");
            if (!jObject.ContainsKey("Relations")) throw new JsonException($"{nameof(DataSetJsonConverter)} Error : 缺少 Relations 属性");
            if (jObject["Relations"] == null || jObject["Relations"]!.Type == JTokenType.Null) throw new JsonException($"{nameof(DataSetJsonConverter)} Error : Relations 为空");
            if (jObject["Relations"]!.Type != JTokenType.Array) throw new JsonException($"{nameof(DataSetJsonConverter)} Error : Relations 类型错误");

            #endregion


            var dataSet = new DataSet();

            if (jObject["CaseSensitive"] != null) dataSet.CaseSensitive = jObject.Value<bool>("CaseSensitive");

            if (jObject["DataSetName"] != null) dataSet.DataSetName = jObject.Value<string>("DataSetName");

            if (jObject["EnforceConstraints"] != null) dataSet.EnforceConstraints = jObject.Value<bool>("EnforceConstraints");

            if (jObject["Namespace"] != null) dataSet.Namespace = jObject.Value<string>("Namespace");

            if (jObject["Prefix"] != null) dataSet.Prefix = jObject.Value<string>("Prefix");


            #region Tables

            var dataTableJsonConverter = new DataTableJsonConverter(_level, _dateTimeFormatType);

            foreach (var jToken in jObject["Tables"]!)
            {
                var tableReader = jToken.CreateReader();

                var convertResult = dataTableJsonConverter.ReadJson(tableReader, typeof(DataTable), null, serializer);
                if (!(convertResult is DataTable dataTable)) throw new JsonException($"{nameof(DataSetJsonConverter)} Error : DataTable 转换失败");
                dataSet.Tables.Add(dataTable);
            }

            #endregion

            #region Relations

            foreach (var jToken in jObject["Relations"]!)
            {
                if (jToken.Type != JTokenType.Object) throw new JsonException($"{nameof(DataTableJsonConverter)} Error : Relations Array 中对象类型错误，必须是 Json{nameof(JTokenType.Object)}");
                var nested          = jToken.Value<bool>("Nested");
                var relationName    = jToken.Value<string>("RelationName");
                var parentTableName = jToken.Value<string>("ParentTableName");
                var childTableName  = jToken.Value<string>("ChildTableName");

                var parentColumns = new List<DataColumn>();

                if (jToken["ParentColumnNames"] != null)
                {
                    foreach (var pCol in jToken["ParentColumnNames"]!)
                    {
                        parentColumns.Add(dataSet.Tables[parentTableName].Columns[pCol.Value<string>()]);
                    }
                }

                var childColumns = new List<DataColumn>();
                if (jToken["ChildColumnNames"] != null)
                {
                    foreach (var cCol in jToken["ChildColumnNames"]!)
                    {
                        childColumns.Add(dataSet.Tables[childTableName].Columns[cCol.Value<string>()]);
                    }
                }

                var relation = new DataRelation(relationName, parentColumns.ToArray(), childColumns.ToArray()) {Nested = nested};
                dataSet.Relations.Add(relation);
            }

            #endregion


            return dataSet;
        }
    }
}