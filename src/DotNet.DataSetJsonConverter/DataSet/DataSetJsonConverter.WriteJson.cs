using Newtonsoft.Json;
using System;
using System.Data;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataSetJsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var dataSet = value as DataSet;
            if (dataSet == null) throw new ArgumentNullException($"无法转换为{nameof(DataSet)}");

            writer.WriteStartObject();

            writer.WritePropertyName("CaseSensitive");
            writer.WriteValue(dataSet.CaseSensitive);
            writer.WritePropertyName("DataSetName");
            writer.WriteValue(dataSet.DataSetName);
            writer.WritePropertyName("EnforceConstraints");
            writer.WriteValue(dataSet.EnforceConstraints);
            writer.WritePropertyName("Namespace");
            writer.WriteValue(dataSet.Namespace);
            writer.WritePropertyName("Prefix");
            writer.WriteValue(dataSet.Prefix);

            #region Relations

            writer.WritePropertyName("Relations");
            writer.WriteStartArray();

            foreach (DataRelation relation in dataSet.Relations)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("Nested");
                writer.WriteValue(relation.Nested);
                
                writer.WritePropertyName("RelationName");
                writer.WriteValue(relation.RelationName);
                
                writer.WritePropertyName("ChildTableName");
                writer.WriteValue(relation.ChildTable.TableName);
                
                writer.WritePropertyName("ParentTableName");
                writer.WriteValue(relation.ParentTable.TableName);
                
                writer.WritePropertyName("ChildColumnNames");
                writer.WriteStartArray();
                foreach (var childColumn in relation.ChildColumns) writer.WriteValue(childColumn.ColumnName);
                writer.WriteEndArray();

                writer.WritePropertyName("ParentColumnNames");
                writer.WriteStartArray();
                foreach (var parentColumn in relation.ParentColumns) writer.WriteValue(parentColumn.ColumnName);
                writer.WriteEndArray();

                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            #endregion

            #region Tables

            writer.WritePropertyName("Tables");
            writer.WriteStartArray();

            var dataTableJsonConverter = new DataTableJsonConverter(_level);
            foreach (DataTable table in dataSet.Tables) dataTableJsonConverter.WriteJson(writer, table, serializer);

            writer.WriteEndArray();

            #endregion


            writer.WriteEndObject();
        }
    }
}
