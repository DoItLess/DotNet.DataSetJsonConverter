using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataTableJsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var table = value as DataTable;
            if (table == null) throw new ArgumentNullException($"无法转换为{nameof(DataTable)}");

            switch (_level)
            {
                case ConvertLevel.Maximal:
                    WriteJsonMaximal(writer, table, serializer);
                    break;
                case ConvertLevel.Normal:
                    WriteJsonNormal(writer, table, serializer);
                    break;
                case ConvertLevel.Minimal:
                    WriteJsonMinimal(writer, table, serializer);
                    break;
                default:
                    throw new ArgumentException($"不存在的{_level}");
            }
        }

        /// <summary>
        /// Minimal
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="table"></param>
        /// <param name="serializer"></param>
        private void WriteJsonMinimal(JsonWriter writer, DataTable table, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("TableName");
            writer.WriteValue(table.TableName);

            #region Columns

            writer.WritePropertyName("Columns");

            writer.WriteStartArray();
            foreach (DataColumn column in table.Columns)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("AllowDBNull");
                writer.WriteValue(column.AllowDBNull);
                writer.WritePropertyName("Caption");
                writer.WriteValue(column.Caption);
                writer.WritePropertyName("ColumnName");
                writer.WriteValue(column.ColumnName);
                writer.WritePropertyName("DataType");
                writer.WriteValue(column.DataType.Name);

                // Extension 
                writer.WritePropertyName("IsPrimaryKey");
                writer.WriteValue(table.PrimaryKey.Contains(column));

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            #endregion

            #region PrimaryKeys

            writer.WritePropertyName("PrimaryKeys");
            writer.WriteStartArray();
            foreach (var pkCol in table.PrimaryKey)
            {
                writer.WriteValue(pkCol.ColumnName);
            }

            writer.WriteEndArray();

            #endregion

            #region Rows

            writer.WritePropertyName("Rows");
            writer.WriteStartArray();
            foreach (DataRow row in table.Rows)
            {
                writer.WriteStartObject();

                foreach (DataColumn col in table.Columns)
                {
                    writer.WritePropertyName(col.ColumnName);
                    writer.WriteValue(row[col.ColumnName]);
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            #endregion


            writer.WriteEndObject();
        }

        /// <summary>
        /// Normal
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="table"></param>
        /// <param name="serializer"></param>
        private void WriteJsonNormal(JsonWriter writer, DataTable table, JsonSerializer serializer)
        {
            writer.WriteStartObject();


            writer.WritePropertyName("Namespace");
            writer.WriteValue(table.Namespace);
            writer.WritePropertyName("TableName");
            writer.WriteValue(table.TableName);

            #region Columns

            writer.WritePropertyName("Columns");

            writer.WriteStartArray();
            foreach (DataColumn column in table.Columns)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("AllowDBNull");
                writer.WriteValue(column.AllowDBNull);
                writer.WritePropertyName("AutoIncrement");
                writer.WriteValue(column.AutoIncrement);
                writer.WritePropertyName("Caption");
                writer.WriteValue(column.Caption);
                writer.WritePropertyName("ColumnName");
                writer.WriteValue(column.ColumnName);
                writer.WritePropertyName("DataType");
                writer.WriteValue(column.DataType.Name);
                writer.WritePropertyName("DefaultValue");
                writer.WriteValue(column.DefaultValue);
                writer.WritePropertyName("MaxLength");
                writer.WriteValue(column.MaxLength);
                writer.WritePropertyName("ReadOnly");
                writer.WriteValue(column.ReadOnly);
                writer.WritePropertyName("Unique");
                writer.WriteValue(column.Unique);

                // Extension 
                writer.WritePropertyName("IsPrimaryKey");
                writer.WriteValue(table.PrimaryKey.Contains(column));

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            #endregion

            #region PrimaryKeys

            writer.WritePropertyName("PrimaryKeys");
            writer.WriteStartArray();
            foreach (var pkCol in table.PrimaryKey)
            {
                writer.WriteValue(pkCol.ColumnName);
            }

            writer.WriteEndArray();

            #endregion

            #region Rows

            writer.WritePropertyName("Rows");
            writer.WriteStartArray();
            foreach (DataRow row in table.Rows)
            {
                writer.WriteStartObject();

                foreach (DataColumn col in table.Columns)
                {
                    writer.WritePropertyName(col.ColumnName);
                    writer.WriteValue(row[col.ColumnName]);
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            #endregion


            writer.WriteEndObject();
        }

        /// <summary>
        /// Maximal
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="table"></param>
        /// <param name="serializer"></param>
        private void WriteJsonMaximal(JsonWriter writer, DataTable table, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("CaseSensitive");
            writer.WriteValue(table.CaseSensitive);
            writer.WritePropertyName("TableName");
            writer.WriteValue(table.TableName);
            writer.WritePropertyName("DisplayExpression");
            writer.WriteValue(table.DisplayExpression);
            writer.WritePropertyName("IsInitialized");
            writer.WriteValue(table.IsInitialized);
            writer.WritePropertyName("MinimumCapacity");
            writer.WriteValue(table.MinimumCapacity);
            writer.WritePropertyName("Namespace");
            writer.WriteValue(table.Namespace);

            #region Columns

            writer.WritePropertyName("Columns");

            writer.WriteStartArray();
            foreach (DataColumn column in table.Columns)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("AutoIncrementSeed");
                writer.WriteValue(column.AutoIncrementSeed);
                writer.WritePropertyName("AutoIncrementStep");
                writer.WriteValue(column.AutoIncrementStep);
                writer.WritePropertyName("AllowDBNull");
                writer.WriteValue(column.AllowDBNull);
                writer.WritePropertyName("AutoIncrement");
                writer.WriteValue(column.AutoIncrement);
                writer.WritePropertyName("Caption");
                writer.WriteValue(column.Caption);
                writer.WritePropertyName("ColumnName");
                writer.WriteValue(column.ColumnName);
                writer.WritePropertyName("DataType");
                writer.WriteValue(column.DataType.Name);
                writer.WritePropertyName("DefaultValue");
                writer.WriteValue(column.DefaultValue);
                writer.WritePropertyName("MaxLength");
                writer.WriteValue(column.MaxLength);
                writer.WritePropertyName("Namespace");
                writer.WriteValue(column.Namespace);
                writer.WritePropertyName("Prefix");
                writer.WriteValue(column.Prefix);
                writer.WritePropertyName("ReadOnly");
                writer.WriteValue(column.ReadOnly);
                writer.WritePropertyName("Unique");
                writer.WriteValue(column.Unique);

                // Extension 
                writer.WritePropertyName("IsPrimaryKey");
                writer.WriteValue(table.PrimaryKey.Contains(column));

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            #endregion

            #region PrimaryKeys

            writer.WritePropertyName("PrimaryKeys");
            writer.WriteStartArray();
            foreach (var pkCol in table.PrimaryKey)
            {
                writer.WriteValue(pkCol.ColumnName);
            }

            writer.WriteEndArray();

            #endregion

            #region Rows

            writer.WritePropertyName("Rows");
            writer.WriteStartArray();
            foreach (DataRow row in table.Rows)
            {
                writer.WriteStartObject();

                foreach (DataColumn col in table.Columns)
                {
                    writer.WritePropertyName(col.ColumnName);
                    writer.WriteValue(row[col.ColumnName]);
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            #endregion

            writer.WriteEndObject();
        }
    }
}