using System;
using System.Data;

namespace DotNet.JsonConverter
{
    public partial class DataColumnConverter : Newtonsoft.Json.JsonConverter
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