using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataColumnJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var column = value as DataColumn;
            if (column == null) throw new ArgumentNullException($"无法转换为{nameof(DataColumn)}");


            switch (_level)
            {
                case ConvertLevel.Maximal:
                    Maximal(writer, column, serializer);
                    break;
                case ConvertLevel.Normal:
                    Normal(writer, column, serializer);
                    break;
                case ConvertLevel.Minimal:
                    Minimal(writer, column, serializer);
                    break;
                default:
                    throw new ArgumentException($"不存在的{_level}");
            }
        }

        private void Maximal(JsonWriter writer, DataColumn column, JsonSerializer serializer)
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
            writer.WriteValue(column.Table.PrimaryKey.Contains(column));

            writer.WriteEndObject();
        }

        private void Normal(JsonWriter writer, DataColumn column, JsonSerializer serializer)
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
            writer.WriteValue(column.Table.PrimaryKey.Contains(column));

            writer.WriteEndObject();
        }

        private void Minimal(JsonWriter writer, DataColumn column, JsonSerializer serializer)
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
            writer.WriteValue(column.Table.PrimaryKey.Contains(column));

            writer.WriteEndObject();
        }


    }
}