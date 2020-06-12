using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataSetJsonConverter
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            if (jObject == null) throw new Exception();

            var dataSet = new DataSet();

            if (jObject["CaseSensitive"] != null) dataSet.CaseSensitive = jObject.Value<bool>("CaseSensitive");

            if (jObject["DataSetName"] != null) dataSet.DataSetName = jObject.Value<string>("DataSetName");

            if (jObject["EnforceConstraints"] != null) dataSet.EnforceConstraints = jObject.Value<bool>("EnforceConstraints");

            if (jObject["Namespace"] != null) dataSet.Namespace = jObject.Value<string>("Namespace");

            if (jObject["Prefix"] != null) dataSet.Prefix = jObject.Value<string>("Prefix");


            if (jObject.ContainsKey("Tables"))
            {
                var dataTableJsonConverter = new DataTableJsonConverter(_level);

                foreach (var jToken in jObject["Tables"])
                {
                    var tableReader = jToken.CreateReader();
                    
                    var dataTable = (DataTable)dataTableJsonConverter.ReadJson(tableReader, typeof(DataTable), null, serializer);
                    
                    dataSet.Tables.Add(dataTable!);
                }
            }

            if (jObject.ContainsKey("Relations"))
            {
                foreach (var jToken in jObject["Relations"])
                {
                    var nested = jToken.Value<bool>("Nested");
                    var relationName = jToken.Value<string>("RelationName");
                    var parentTableName = jToken.Value<string>("ParentTableName");
                    var childTableName = jToken.Value<string>("ChildTableName");

                    var parentColumns = new List<DataColumn>();
                    foreach (var pCol in jToken["ParentColumnNames"])
                    {
                        var colName = pCol.Value<string>();
                        parentColumns.Add(dataSet.Tables[parentTableName].Columns[colName]);
                    }

                    var childColumns = new List<DataColumn>();
                    foreach (var cCol in jToken["ChildColumnNames"])
                    {
                        var colName = cCol.Value<string>();
                        childColumns.Add(dataSet.Tables[childTableName].Columns[colName]);
                    }

                    var relation = new DataRelation(relationName,parentColumns.ToArray(), childColumns.ToArray());


                    dataSet.Relations.Add(relation);
                }
            }

            return dataSet;
        }
    }
}