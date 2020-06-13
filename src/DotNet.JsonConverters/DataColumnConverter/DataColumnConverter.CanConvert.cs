using System;
using System.Data;
using Newtonsoft.Json;

namespace DotNet.JsonConverters
{
    public partial class DataColumnConverter : JsonConverter
    {
        private readonly ConvertLevel _level;

        public DataColumnConverter(ConvertLevel level = ConvertLevel.Normal)
        {
            _level = level;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataRow).IsAssignableFrom(objectType);
        }
    }
}