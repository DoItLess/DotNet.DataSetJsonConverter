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

            
            foreach (DataRelation dataSetRelation in dataSet.Relations)
            {
                
            }

            #endregion

            #region Tables

            foreach (DataTable dataSetTable in dataSet.Tables)
            {
                
            }

            #endregion

            writer.WriteEndObject();
        }
    }
}
