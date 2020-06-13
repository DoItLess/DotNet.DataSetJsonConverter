using Newtonsoft.Json;
using System;
using System.Data;

// ReSharper disable UnusedParameter.Local

namespace DotNet.JsonConverters
{
    public partial class DataTableConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var table = value as DataTable;
            if (table == null) throw new JsonException($"{nameof(DataTableConverter)} Error : 无法转换为{nameof(DataTable)}");

            switch (_level)
            {
                case ConvertLevel.Maximal:
                    Maximal(writer, table, serializer, _level);
                    break;
                case ConvertLevel.Normal:
                    Normal(writer, table, serializer, _level);
                    break;
                case ConvertLevel.Minimal:
                    Minimal(writer, table, serializer, _level);
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
        /// <param name="level"></param>
        private void Minimal(JsonWriter writer, DataTable table, JsonSerializer serializer, ConvertLevel level)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("TableName");
            writer.WriteValue(table.TableName);
            writer.WritePropertyName("Namespace");
            writer.WriteValue(table.Namespace);

            #region Columns

            writer.WritePropertyName("Columns");

            writer.WriteStartArray();

            var dataColumnJsonConverter = new DataColumnConverter(level);
            foreach (DataColumn column in table.Columns) dataColumnJsonConverter.WriteJson(writer, column, serializer);

            writer.WriteEndArray();

            #endregion

            #region PrimaryKeys

            writer.WritePropertyName("PrimaryKeys");
            writer.WriteStartArray();
            foreach (var pkCol in table.PrimaryKey) writer.WriteValue(pkCol.ColumnName);

            writer.WriteEndArray();

            #endregion

            #region Rows

            WriteRows(writer, table, serializer, level);

            #endregion


            writer.WriteEndObject();
        }

        /// <summary>
        /// Normal
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="table"></param>
        /// <param name="serializer"></param>
        /// <param name="level"></param>
        private void Normal(JsonWriter writer, DataTable table, JsonSerializer serializer, ConvertLevel level)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Namespace");
            writer.WriteValue(table.Namespace);
            writer.WritePropertyName("TableName");
            writer.WriteValue(table.TableName);

            #region Columns

            writer.WritePropertyName("Columns");

            writer.WriteStartArray();

            var dataColumnJsonConverter = new DataColumnConverter(level);
            foreach (DataColumn column in table.Columns) dataColumnJsonConverter.WriteJson(writer, column, serializer);

            writer.WriteEndArray();

            #endregion

            #region PrimaryKeys

            writer.WritePropertyName("PrimaryKeys");
            writer.WriteStartArray();
            foreach (var pkCol in table.PrimaryKey) writer.WriteValue(pkCol.ColumnName);

            writer.WriteEndArray();

            #endregion

            #region Rows

            WriteRows(writer, table, serializer, level);

            #endregion


            writer.WriteEndObject();
        }

        /// <summary>
        /// Maximal
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="table"></param>
        /// <param name="serializer"></param>
        /// <param name="level"></param>
        private void Maximal(JsonWriter writer, DataTable table, JsonSerializer serializer, ConvertLevel level)
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

            var dataColumnJsonConverter = new DataColumnConverter(level);
            foreach (var column in table.Columns) dataColumnJsonConverter.WriteJson(writer, column, serializer);

            writer.WriteEndArray();

            #endregion

            #region PrimaryKeys

            writer.WritePropertyName("PrimaryKeys");
            writer.WriteStartArray();
            foreach (var pkCol in table.PrimaryKey) writer.WriteValue(pkCol.ColumnName);

            writer.WriteEndArray();

            #endregion

            #region Rows

            WriteRows(writer, table, serializer, level);

            #endregion

            writer.WriteEndObject();
        }

        private void WriteRows(JsonWriter writer, DataTable table, JsonSerializer serializer, ConvertLevel level)
        {
            writer.WritePropertyName("Rows");
            writer.WriteStartArray();
            foreach (DataRow row in table.Rows)
            {
                writer.WriteStartObject();

                foreach (DataColumn col in table.Columns)
                {
                    writer.WritePropertyName(col.ColumnName);

                    var isSpecial = Type.GetTypeCode(col.DataType) == TypeCode.DateTime && _dateTimeFormatStyle == DateTimeFormatStyle.TimeStampMillisecond;

                    var value = isSpecial
                        ? DateTimeToMilliseconds(Convert.ToDateTime(row[col.ColumnName]))
                        : row[col.ColumnName];
                    writer.WriteValue(value);
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}