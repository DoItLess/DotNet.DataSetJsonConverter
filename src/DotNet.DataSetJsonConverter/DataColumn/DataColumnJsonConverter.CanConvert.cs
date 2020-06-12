using System;
using System.Data;
using Newtonsoft.Json;

namespace DotNet.DataSetJsonConverter
{
    public partial class DataColumnJsonConverter : JsonConverter
    {
        private readonly ConvertLevel _level;

        public DataColumnJsonConverter(ConvertLevel level = ConvertLevel.Normal)
        {
            _level = level;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataRow).IsAssignableFrom(objectType);
        }
    }
}